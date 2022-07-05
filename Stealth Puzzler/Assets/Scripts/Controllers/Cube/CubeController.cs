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
    
    [Header("Grid Snapping")]
    [SerializeField] private float _snapSpeed = .3f;

    [Tooltip("Transform for syncing cube and player position on switch.")]
    public Transform PlayerCalibratorTransform;

    [Header("Ground Check")]
    [SerializeField] private LayerMask _groundLayerMask;
    
    public Rigidbody Rigidbody;
    private List<Vector3> _directions = new List<Vector3>();
    private Vector3 _newdirection;

    private GroundCheck _groundCheck { get; set; }
    private Grid _grid;

    public float FallTimer { get; set; }
    [field: SerializeField] public bool IsFalling { get; set; }
    
    private float _horizontal;
    private float _vertical;
    private bool _isMoving;
    private float _weight;
    private bool _touchingGround;
    private bool _rotated45 = false;

    private void OnEnable()
    {
        _move.action.Enable();
        _turnLeft.action.performed += TurnLeft;
        _turnRight.action.performed += TurnRight;
        SnapToGrid();
    }

    private void TurnLeft(InputAction.CallbackContext context)
    {
        if (_isMoving) return;
        StartCoroutine(RotateCube(-45));
    }

    private void TurnRight(InputAction.CallbackContext context)
    {
        if (_isMoving) return;
        StartCoroutine(RotateCube(45));
    }

    private IEnumerator RotateCube(int degrees)
    {
        if (_isMoving) yield break;
        _isMoving = true;
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
        _isMoving = false;
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
        
        //Initialize directions to snap to
        _directions.Add(Vector3.forward);
        _directions.Add(Vector3.back);
        _directions.Add(Vector3.right);
        _directions.Add(Vector3.left);
    }

    private void Update()
    {
        ReadInput();
        if (_isMoving) return;
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
        return _isMoving;
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
        IsTouchingGround();
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

        PlayTumbleSound();

        _newdirection = _directions[closestAxis];
        var anchor = Rigidbody.transform.position + (Vector3.down + _newdirection) * 0.5f;
        var axis = Vector3.Cross(Vector3.up, _newdirection);
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
        _isMoving = true;
        float rotationRemaining = 90;

        while (rotationRemaining > 0)
        {
            float rotateAmount = Mathf.Min(Time.deltaTime * _rollSpeed, rotationRemaining);
            Rigidbody.transform.RotateAround(anchor, axis, rotateAmount);
            rotationRemaining -= rotateAmount;
            yield return null;
        }

        SnapToGrid();
        _isMoving = false;
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
    }
}