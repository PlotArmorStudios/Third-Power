using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : Controller
{
    [Header("Input")] [SerializeField] private InputActionReference _move;
    [SerializeField] private InputActionReference _jump;
    [SerializeField] private InputActionReference _look;
    [SerializeField] private InputActionReference _run;

    [Header("Movement")] [SerializeField] private float _maxMovementSpeed = 5f;
    [SerializeField] private float _jumpHeight = 10f;
    [SerializeField] private float _weight = 2f;

    

    [SerializeField] private float _turnSmoothTime = 2f;
    [SerializeField] private float _turnSmoothVelocity = 2f;

    [Tooltip("Transform for syncing player and cube position on switch.")]
    public Transform CubeCalibratorTransform;

    [field: SerializeField] public bool IsFalling { get; set; }

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
    private float _currentMovementSpeed;

    //Running
    private bool _isRunning;
    private float _speedVariant;

    //Jumping
    private bool _triggerJump;
    public bool IsJumping { get; set; }

    //Vulnerability
    public bool IsVulnerable { get; private set; }
    private float _vulnerableTime;
    
    //Wall Climbing
    [SerializeField] private LayerMask _climbMask;
    [SerializeField] private Transform[] _climbCheckPoints;
    [SerializeField] private bool _isClimbing = false;
    [SerializeField] [Range(200, 500)] private float _climbSpeed = 300;
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

        IsVulnerable = false;
        _vulnerableTime = 0;
    }

    private void OnDisable()
    {
        _jump.action.Disable();
        _look.action.Disable();
        _run.action.Disable();
    }

    void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponentInChildren<Animator>();
        _groundCheck = GetComponent<GroundCheck>();
    }

    void Update()
    {
        MakeVulnerable();
        ReadInput();
        ApplyMovementInputToAnimator();
        ToggleAirborneState();
        if (PlayerJumpedFromGround()) _triggerJump = true;
        CheckIfClimbing();
    }

    private void MakeVulnerable()
    {
        _vulnerableTime += Time.deltaTime;

        if (_vulnerableTime > .2f)
            IsVulnerable = true;
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
        
        HandleLand();
    }

    private void HandleLand()
    {
        if (_groundCheck.IsGrounded() && FallTimer > 0)
        {
            FallTimer = 0;
            _animator.SetBool("Airborne", false);
            _animator.ResetTrigger("Jump");
            _animator.SetTrigger("Land");
        }
    }

    private void ApplyMovementInputToAnimator()
    {
        var input = Mathf.Max(Mathf.Abs(_horizontal),
            Mathf.Abs(_vertical));
        _isRunning = _run.action.IsPressed();

        if (input > 0.1f)
        {
            if (_isRunning)
            {
                _speedVariant += Time.deltaTime;
                _speedVariant = Mathf.Clamp(_speedVariant, 0, 1f);
            }
            else
            {
                if (_speedVariant < .5f)
                    _speedVariant += Time.deltaTime;
                if (_speedVariant > .5f)
                    _speedVariant -= Time.deltaTime;
                _speedVariant = Mathf.Clamp(_speedVariant, 0, 1f);
            }
        }
        else
        {
            _speedVariant -= Time.deltaTime;
            _speedVariant = Mathf.Clamp(_speedVariant, 0, .5f);
        }

        _animator.SetFloat("Movement", _speedVariant);
    }

    private void ToggleAirborneState()
    {
        _animator.SetBool("Animator", !_groundCheck.IsGrounded());
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
            _currentMovementSpeed += Time.deltaTime;
            CalculateMovementDirection();
        }
        else
        {
            //let character jump while stopping sliding
            //character only stops completely when grounded
            //set airborne false whenever grounded

            if (_groundCheck.IsGrounded())
                Rigidbody.velocity = new Vector3(0f, Rigidbody.velocity.y, 0f);

            _currentMovementSpeed -= Time.deltaTime;
        }

        _currentMovementSpeed = Mathf.Clamp(_currentMovementSpeed, 3.8f, _maxMovementSpeed);
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

        if (_isRunning) moving = moveDir.normalized * (_currentMovementSpeed * 1.5f);
        else moving = moveDir.normalized * _currentMovementSpeed;

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
        return Mathf.Atan2(_movement.x, _movement.z) * Mathf.Rad2Deg + CamTransform.localEulerAngles.y;
    }

    private void HandleJump()
    {
        if (_triggerJump)
        {
            _animator.ResetTrigger("Land");
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