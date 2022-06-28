using System;
using UnityEngine;

public class Climb : MonoBehaviour
{
    //Wall Climbing
    public LayerMask _climbMask;
    [SerializeField] private Transform[] _climbCheckPoints;
    public bool IsClimbing = false;
    [SerializeField] [Range(200, 500)] private float _climbSpeed = 300;
    public float _stoppingDistance = 0.2f;
    [SerializeField] private float _climbJumpForce = 300f;
    private RaycastHit _lastGrabPoint;
    private float _distanceToWall;

    private Rigidbody Rigidbody;
    private PlayerController _playerController;
    private Animator _animator;
    private float _climbTime;

    private void Start()
    {
        _playerController = GetComponent<PlayerController>();
        Rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    public void CheckIfClimbing()
    {
        //Using a big or instead of a for loop so that if one checksphere succeeds, it won't waste time trying the rest
        IsClimbing = (Physics.CheckSphere(_climbCheckPoints[0].position, _stoppingDistance, _climbMask)
                      || Physics.CheckSphere(_climbCheckPoints[1].position, _stoppingDistance, _climbMask)
                      || Physics.CheckSphere(_climbCheckPoints[2].position, _stoppingDistance, _climbMask));

        if (IsClimbing)
        {
            if (_climbTime == 0)
            {
                Debug.Log("Set Climb anim");
                _animator.ResetTrigger("Set Climb");
                _animator.ResetTrigger("Unset Climb");
                _animator.SetTrigger("Set Climb");
            }

            _climbTime += Time.deltaTime;
        }

        if (!IsClimbing)
        {
            if (_climbTime > 0)
            {
                Debug.Log("Unset climb anim");
                _animator.ResetTrigger("Set Climb");
                _animator.ResetTrigger("Unset Climb");
                _animator.SetTrigger("Unset Climb");
            }

            _climbTime = 0;
        }
        if (Rigidbody != null)
        {
            Rigidbody.useGravity = !IsClimbing;
        }
    }

    public void ToggleOnClimbState()
    {
        _animator.SetTrigger("Climb");
    }

    public void ToggleOffClimbState()
    {
        _animator.SetTrigger("Stop Climb");
    }
    
    public void HandleWallClimbing()
    {
        if (_playerController.Jump.action.triggered)
        {
            Debug.Log("Yumped");
            
            if (_playerController.Vertical < 0)
            {
                transform.forward = -transform.forward;
                WallJump();
                return;
            }
            if (_playerController.Horizontal < 0)
            {
                transform.forward = -transform.right;
                WallJump();
                return;
            }
            if (_playerController.Horizontal > 0)
            {
                transform.forward = transform.right;
                WallJump();
                return;
            }
        }

        if (_playerController.Vertical < 0 && _playerController.GroundCheck.IsGrounded())
        {
            _playerController.RotateInDirectionOfMovement();
            return;
        }

        _playerController.IsFalling = false;
        _playerController.FallTimer = 0;

        RootMotionClimb(_playerController.Horizontal, _playerController.Vertical);
        
        if (_playerController.Horizontal != 0 || _playerController.Vertical != 0)
        {
            //RawClimb();
        }
        else
        {
            Rigidbody.velocity = Vector3.zero;
        }
    }

    private void RootMotionClimb(float horizontal, float vertical)
    {
        _animator.SetFloat("Climb X", horizontal);
        _animator.SetFloat("Climb Y", vertical);
    }
    
    private void RawClimb()
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
        Vector3 movePos = (transform.right * _playerController.Horizontal + transform.up * _playerController.Vertical)
            .normalized;
        Rigidbody.velocity = movePos * _climbSpeed * Time.fixedDeltaTime;
    }

    private void WallJump()
    {
        transform.position += transform.forward / 10;
        Rigidbody.velocity = (transform.forward + transform.up) * _climbJumpForce;
    }
}