using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public Transform CubeCalibratorTransform;

    [SerializeField] private InputActionReference _move;
    [SerializeField] private InputActionReference _jump;
    [SerializeField] private InputActionReference _look;
    [SerializeField] private float _jumpHeight = 10f;

    [SerializeField] private float _movementSpeed = 5.0f;
    [SerializeField] private float _weight = 2f;
    [SerializeField] private float _turnSmoothTime = 2f;
    [SerializeField] private float _turnSmoothVelocity = 2f;

    [SerializeField] private Camera _camera;
    [field: SerializeField] public bool IsFalling { get; set; }

    //Camera for calculation movement direction
    private Transform CamTransform;
    public Animator Animator => _animator;

    private Rigidbody _rigidbody;
    private Animator _animator;

    public GroundCheck GroundCheck { get; private set; }
    public float FallTimer { get; set; }

    //Movement
    private Vector3 _movement;
    private Vector3 _heightMovement;
    private float _horizontal;
    private float _vertical;

    //Jumping
    private bool _triggerJump;
    public bool IsJumping { get; set; }

    private void OnEnable()
    {
        _move.action.Enable();
        _jump.action.Enable();
        _look.action.Enable();
    }

    private void OnDisable()
    {
        _jump.action.Disable();
        _look.action.Disable();
    }

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        GroundCheck = GetComponent<GroundCheck>();

        CamTransform = _camera.transform;
    }

    void Update()
    {
        ReadInput();
        //ApplyMovementInputToAnimator();
        if (PlayerJumpedFromGround()) _triggerJump = true;
    }

    private void FixedUpdate()
    {
        RotateInDirectionOfMovement();
        ApplyGravity();
        UpDateJump();
        HandleJump();
    }

    private void ReadInput()
    {
        _horizontal = _move.action.ReadValue<Vector2>().x;
        _vertical = _move.action.ReadValue<Vector2>().y;
    }

    private void UpDateJump()
    {
        if (GroundCheck.UpdateIsGrounded())
            IsJumping = false;

        if (GroundCheck.UpdateIsGrounded() && FallTimer > 0)
        {
            FallTimer = 0;
            //_animator.SetBool("Airborne", false);
            // _animator.SetTrigger("Land");
        }
    }

    private void ApplyMovementInputToAnimator()
    {
        _animator.SetFloat("Movement",
            Mathf.Max(Mathf.Abs(_horizontal),
                Mathf.Abs(_vertical)));
    }

    private bool PlayerJumpedFromGround()
    {
        return _jump.action.triggered && GroundCheck.IsGrounded;
    }

    private void RotateInDirectionOfMovement()
    {
        //have player face the direction the camera is facing only if they are moving
        _movement = new Vector3(_horizontal, 0, _vertical);
        if (_movement.magnitude >= .3f)
        {
            CalculateMovementDirection();
        }
        else
        {
            //let character jump while stopping sliding
            //character only stops completely when grounded
            //set airborne false whenever grounded
            if (GroundCheck.IsGrounded)
            {
                _rigidbody.velocity = new Vector3(0f, _rigidbody.velocity.y, 0f);
            }
        }
    }

    private void CalculateMovementDirection()
    {
        //rotate movement direction based on camera rotation
        float targetAngle = CalculateAngleWithCam();

        if (Mathf.Abs(_look.action.ReadValue<Vector2>().x) > 0)
        {
            _turnSmoothTime = .3f;
        }
        else
        {
            _turnSmoothTime = .075f;
        }

        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity,
            _turnSmoothTime);

        Vector3 moveDir;

        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        //the angle that the character is moving * the actual movement itself
        moveDir = Quaternion.Euler(0f, angle, 0f) * Vector3.forward;

        var moving = moveDir.normalized * _movementSpeed;

        //how to have character face direction you are moving
        if (!IsJumping)
        {
            _rigidbody.velocity = new Vector3(moving.x, _rigidbody.velocity.y, moving.z);
        }
        else if (IsJumping)
        {
            //aerial mobility
            _rigidbody.AddForce(.1f * moveDir, ForceMode.VelocityChange);
        }
    }

    private float CalculateAngleWithCam()
    {
        return Mathf.Atan2(_movement.x, _movement.z) * Mathf.Rad2Deg + CamTransform.eulerAngles.y;
    }

    private void HandleJump()
    {
        if (_triggerJump)
        {
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY
                                                                          | RigidbodyConstraints.FreezeRotationZ;
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _jumpHeight, _rigidbody.velocity.z);
            IsJumping = true; //for landing
            //_animator.SetBool("Land", false);
            //_animator.SetTrigger("Jump");
            _triggerJump = false;
        }
    }

    private void ApplyGravity()
    {
        if (!GroundCheck.IsGrounded)
        {
            FallTimer += Time.deltaTime;
            var downForce = _weight * FallTimer * FallTimer;

            if (downForce > 3f)
                downForce = 3f;

            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y - downForce,
                _rigidbody.velocity.z);

            if (_rigidbody.velocity.y < 0)
                IsFalling = true;
            _animator.SetBool("Airborne", true);
        }
        else
        {
            FallTimer = 0;
            IsFalling = false;
        }
    }
}