using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private InputActionReference _move;
    [SerializeField] private InputActionReference _jump;
    [SerializeField] private InputActionReference _look;
    [SerializeField] private InputActionReference _run;

    [Header("Movement")] [SerializeField] private float _movementSpeed = 5.0f;
    [SerializeField] private float _jumpHeight = 10f;
    [SerializeField] private float _weight = 2f;

    [Header("Movement/Cam Rotation Sync")] [SerializeField]
    private Camera _camera;

    [SerializeField] private float _turnSmoothTime = 2f;
    [SerializeField] private float _turnSmoothVelocity = 2f;

    [Tooltip("Transform for syncing player and cube position on switch.")]
    public Transform CubeCalibratorTransform;

    [field: SerializeField] public bool IsFalling { get; set; }

    //Camera for calculating movement direction
    private Transform CamTransform;

    public Rigidbody Rigidbody { get; private set; }
    private Animator _animator;

    //Gravity
    private GroundCheck _groundCheck { get; set; }
    public float FallTimer { get; set; }

    //Movement
    private Vector3 _movement;
    private Vector3 _heightMovement;
    private float _horizontal;
    private float _vertical;
    
    //Running
    private bool _isRunning;

    //Jumping
    private bool _triggerJump;
    public bool IsJumping { get; set; }

    
    //Wall Climbing
    [SerializeField] private LayerMask _climbMask;
    [SerializeField] private Transform[] _climbCheckPoints;
    [SerializeField] private bool _isClimbing = false;
    [SerializeField][Range(200,500)] private float _climbSpeed = 300;
    [SerializeField] private float _stoppingDistance = 0.2f;
    [SerializeField] private float _climbJumpForce = 300f;
    private RaycastHit _lastGrabPoint; 
    private float _distanceToWall;

    private void OnEnable()
    {
        _move.action.Enable();
        _jump.action.Enable();
        _look.action.Enable();
        _run.action.Enable();
    }

    private void OnDisable()
    {
        _jump.action.Disable();
        _look.action.Disable();
        _run.action.Disable();
    }

    void Start()
    {
        _camera = Camera.main;
        Rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _groundCheck = GetComponent<GroundCheck>();

        CamTransform = _camera.transform;
    }

    void Update()
    {
        ReadInput();
        ApplyMovementInputToAnimator();
        if (PlayerJumpedFromGround()) _triggerJump = true;
        CheckIfClimbing();
    }


    private void FixedUpdate()
    {
        if (_isClimbing)
        {
            HandleWallClimbing();
            return;
        }

        RotateInDirectionOfMovement();
        UpdateJump();
        ApplyGravity();
        HandleJump();

    }


    private void ReadInput()
    {
        _horizontal = _move.action.ReadValue<Vector2>().x;
        _vertical = _move.action.ReadValue<Vector2>().y;
    }

    private void UpdateJump()
    {
        if (_groundCheck.IsGrounded())
            IsJumping = false;

        if (_groundCheck.IsGrounded() && IsFalling)
        {
            FallTimer = 0;
            _animator.SetBool("Airborne", false);
            _animator.SetTrigger("Land");
        }
    }

    private void ApplyMovementInputToAnimator()
    {
        var speedVariant = Mathf.Max(Mathf.Abs(_horizontal),
            Mathf.Abs(_vertical));

        _isRunning = _run.action.IsPressed();
        
        if (_isRunning)
            speedVariant = Mathf.Clamp(speedVariant, 0, 1f);
        else
            speedVariant = Mathf.Clamp(speedVariant, 0, .5f);
        
        _animator.SetFloat("Movement", speedVariant);
    }

    private bool PlayerJumpedFromGround()
    {
        return _jump.action.triggered && _groundCheck.IsGrounded();
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
            if (_groundCheck.IsGrounded())
            {
                Rigidbody.velocity = new Vector3(0f, Rigidbody.velocity.y, 0f);
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

        var moving = Vector3.zero;
        
        if(_isRunning) moving = moveDir.normalized * (_movementSpeed * 1.5f);
        else moving = moveDir.normalized * _movementSpeed;

        //how to have character face direction you are moving
        if (!IsJumping)
        {
            Rigidbody.velocity = new Vector3(moving.x, Rigidbody.velocity.y, moving.z);
        }
        else if (IsJumping)
        {
            //aerial mobility
            Rigidbody.AddForce(.1f * moveDir, ForceMode.VelocityChange);
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
            Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY
                                                                          | RigidbodyConstraints.FreezeRotationZ;
            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, _jumpHeight, Rigidbody.velocity.z);
            IsJumping = true; //for landing
            _animator.SetTrigger("Jump");
            _triggerJump = false;
        }
    }

    private void CheckIfClimbing()
    {
        //Using a big or instead of a for loop so that if one checksphere succeeds, it won't waste time trying the rest
        _isClimbing = (Physics.CheckSphere(_climbCheckPoints[0].position, _stoppingDistance, _climbMask) 
                    || Physics.CheckSphere(_climbCheckPoints[1].position, _stoppingDistance, _climbMask) 
                    || Physics.CheckSphere(_climbCheckPoints[2].position, _stoppingDistance, _climbMask));
        Rigidbody.useGravity = !_isClimbing;
    }

    private void HandleWallClimbing()
    {
        if (_jump.action.triggered)
        {
            Debug.Log("Yumped");
            if (_vertical < 0)
            {
                transform.forward = -transform.forward;
                WallJump();
                return;
            }
            else if (_horizontal < 0)
            {
                transform.forward = -transform.right;
                WallJump();
                return;
            }
            else if (_horizontal > 0)
            {
                transform.forward = transform.right;
                WallJump();
                return;
            }
        }

        if (_vertical < 0 && _groundCheck.IsGrounded())
        {
            RotateInDirectionOfMovement();
            return;
        }

        IsFalling = false;
        FallTimer = 0;

        if (_horizontal != 0 || _vertical != 0)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 3f, _climbMask))
            {
                _lastGrabPoint = hit;
                Vector3 endPoint = hit.point + -transform.forward.normalized * _stoppingDistance;
                //_rigidbody.position = endPoint;
            }
            
            transform.forward = -_lastGrabPoint.normal;
            //TODO when raycast no longer hit, position is 
            Vector3 movePos = (transform.right * _horizontal + transform.up * _vertical).normalized;
            Rigidbody.velocity = movePos * _climbSpeed * Time.fixedDeltaTime;
        }   
        else
        {
            Rigidbody.velocity = Vector3.zero;
        }
    }

    private void WallJump()
    {
        transform.position += transform.forward / 10;
        Rigidbody.velocity = (transform.forward + transform.up) * _climbJumpForce;
    }

    private void ApplyGravity()
    {
        if (!_groundCheck.IsGrounded() && !_isClimbing)
        {
            FallTimer += Time.deltaTime;
            var downForce = _weight * FallTimer * FallTimer;

            if (downForce > 3f)
                downForce = 3f;

            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, Rigidbody.velocity.y - downForce,
                Rigidbody.velocity.z);

            if (Rigidbody.velocity.y < 0)
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