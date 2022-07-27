//#define DEBUGLOG

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeController : Controller
{
    [SerializeField] private InputActionReference _move;
    [SerializeField] private InputActionReference _turnLeft;
    [SerializeField] private InputActionReference _turnRight;
    [SerializeField] private float _rollSpeed = 5;
    [SerializeField] private float _rotationDuration = 0.2f;

    [Header("Grid Snapping")] [SerializeField]
    private float _snapSpeed = .3f;

    [Tooltip("Transform for syncing cube and player position on switch.")]
    public Transform PlayerCalibratorTransform;

    [Header("Ground Check")] [SerializeField]
    private LayerMask _groundLayerMask;

    [Header("Wall Check")] [SerializeField]
    private LayerMask _wallMask;

    [SerializeField] private float _wallSensorLength;

    public Rigidbody Rigidbody;
    private List<Vector3> _directions = new List<Vector3>();
    private Vector3 _newdirection;

    private GroundCheck _groundCheck { get; set; }
    private Grid _grid;
    public Drop Drop { get; set; }

    public float FallTimer { get; set; }
    [field: SerializeField] public bool IsFalling { get; set; }

    private float _horizontal;
    private float _vertical;
    public bool IsMoving { get; set; }
    private float _weight;
    private bool _touchingGround;
    private bool _rotated45 = false;

    private bool _touchingWallNorth;
    private bool _touchingWallSouth;
    private bool _touchingWallEast;
    private bool _touchingWallWest;

    private Vector3 _north = Vector3.forward;
    private Vector3 _south = Vector3.back;
    private Vector3 _east = Vector3.right;
    private Vector3 _west = Vector3.left;

    private void OnEnable()
    {
        _move.action.Enable();
        _turnLeft.action.performed += TurnLeft;
        _turnRight.action.performed += TurnRight;
        ControllerManager.Instance.DeactivatePlayer();
        SnapToGrid();
    }

    private void OnDisable()
    {
        ControllerManager.Instance.ActivatePlayer();
    }

    private void TurnLeft(InputAction.CallbackContext context)
    {
        if (IsMoving) return;
        StartCoroutine(RotateCube(-45));
    }

    private void TurnRight(InputAction.CallbackContext context)
    {
        if (IsMoving) return;
        StartCoroutine(RotateCube(45));
    }

    private IEnumerator RotateCube(int degrees)
    {
        if (IsMoving) yield break;
        IsMoving = true;
        PlayTumbleSound();
        _rotated45 = !_rotated45;
        Quaternion startingRot = transform.rotation;
        Quaternion targetRot = Quaternion.Euler(transform.rotation.eulerAngles + (Vector3.up * degrees));
        float counter = 0;
        while (counter < _rotationDuration)
        {
            counter += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(startingRot, targetRot, counter / _rotationDuration);
            yield return null;
        }

        transform.rotation = targetRot;
        IsMoving = false;
    }

    private void OnValidate()
    {
        Cam = Camera.main;
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        _grid = FindObjectOfType<Grid>();
    }

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        Cam = Camera.main;
        Drop = GetComponent<Drop>();

        //Initialize directions to snap to
        _directions.Add(Vector3.forward);
        _directions.Add(Vector3.back);
        _directions.Add(Vector3.right);
        _directions.Add(Vector3.left);
    }

    private void Update()
    {
        ReadInput();

        if (IsMoving) return;
        //if cube is at a 45° rotation, turn instead of moving
        if (_rotated45)
        {
            ResetRotation();
            return;
        }

        if (_horizontal <= -0.1f) Tumble(-Cam.transform.right);
        else if (_horizontal >= 0.1f) Tumble(Cam.transform.right);
        else if (_vertical >= 0.1f) Tumble(Cam.transform.forward);
        else if (_vertical <= -0.1f) Tumble(-Cam.transform.forward);

        if (IsFalling) FallTimer += Time.deltaTime;
        else FallTimer = 0;
    }

    public bool GetIsMoving()
    {
        return IsMoving;
    }

    private void ResetRotation()
    {
        if (_horizontal <= -0.1f) StartCoroutine(RotateCube(-45));
        else if (_horizontal >= 0.1f) StartCoroutine(RotateCube(45));
        else if (_vertical >= 0.1f) StartCoroutine(RotateCube(45));
        else if (_vertical <= -0.1f) StartCoroutine(RotateCube(-45));
    }

    private void FixedUpdate()
    {
        CheckWallCollision();
        IsTouchingGround();
    }

    private void CheckWallCollision()
    {
        RaycastHit hit;

        _touchingWallNorth = Physics.Raycast(transform.position, _north, out hit, _wallSensorLength, _wallMask);
        _touchingWallSouth = Physics.Raycast(transform.position, _south, out hit, _wallSensorLength, _wallMask);
        _touchingWallEast = Physics.Raycast(transform.position, _east, out hit, _wallSensorLength, _wallMask);
        _touchingWallWest = Physics.Raycast(transform.position, _west, out hit, _wallSensorLength, _wallMask);
    }

    public bool IsTouchingGround()
    {
        _touchingGround = Physics.Raycast(Rigidbody.transform.position, Vector3.down, 1.3f, _groundLayerMask);
        return _touchingGround;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        Gizmos.DrawRay(Rigidbody.transform.position, Vector3.down * 1.3f);
    }

    private void Tumble(Vector3 dir)
    {
        float largestDot = 0;
        int closestAxis = 0;

        for (int currentAxis = 0; currentAxis < _directions.Count; currentAxis++)
        {
            var dot = Vector3.Dot(dir, _directions[currentAxis]);
            if (dot > largestDot)
            {
                largestDot = dot;
                closestAxis = currentAxis;
            }
        }


        _newdirection = _directions[closestAxis];

        if (_touchingWallNorth && _newdirection == Vector3.forward) return;
        if (_touchingWallSouth && _newdirection == Vector3.back) return;
        if (_touchingWallEast && _newdirection == Vector3.right) return;
        if (_touchingWallWest && _newdirection == Vector3.left) return;

        var anchor = Rigidbody.transform.position + (Vector3.down + _newdirection) * 0.5f;
        var axis = Vector3.Cross(Vector3.up, _newdirection);
        PlayTumbleSound();
        StartCoroutine(Roll(anchor, axis));
    }

    private void ReadInput()
    {
        _horizontal = _move.action.ReadValue<Vector2>().x;
        _vertical = _move.action.ReadValue<Vector2>().y;
    }

    private IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
#if UNITY_EDITOR
        float currTime = Time.time;
#endif

        //enable the ability to be detected by enemies
        ControllerManager.Instance.ActivatePlayer();

        IsMoving = true;
        float rotationRemaining = 90;

        while (rotationRemaining > 0)
        {
            float rotateAmount = Mathf.Min(Time.deltaTime * _rollSpeed, rotationRemaining);
            Rigidbody.transform.RotateAround(anchor, axis, rotateAmount);
            rotationRemaining -= rotateAmount;
            yield return null;
        }

        SnapToGrid();
        IsMoving = false;

        //disable the ability to be detected by enemies
        ControllerManager.Instance.DeactivatePlayer();
#if UNITY_EDITOR && DEBUGLOG
        Debug.Log("Roll took " + (Time.time - currTime) + "seconds");
#endif
    }

    private void PlayTumbleSound()
    {
        AkSoundEngine.PostEvent("Play_Cube_Movement", gameObject);
    }

    private void SnapToGrid()
    {
        Vector3 snappedPostion = _grid.GetNearestPointOnGrid(Rigidbody.transform.position);
        snappedPostion.y = Rigidbody.transform.position.y;
        Rigidbody.transform.position = Vector3.Lerp(Rigidbody.transform.position, snappedPostion, _snapSpeed);
#if DEBUGLOG
        Debug.Log("Snapped to " + _grid.GetNearestPointOnGrid(Rigidbody.transform.position));
#endif
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(Rigidbody.transform.position, _newdirection * 5);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(Cam.transform.position, Cam.transform.forward * 10);

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(transform.position, _north * _wallSensorLength);
        Gizmos.DrawRay(transform.position, _south * _wallSensorLength);
        Gizmos.DrawRay(transform.position, _east * _wallSensorLength);
        Gizmos.DrawRay(transform.position, _west * _wallSensorLength);
    }
}