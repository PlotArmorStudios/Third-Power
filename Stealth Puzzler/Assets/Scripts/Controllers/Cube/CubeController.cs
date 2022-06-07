//#define DEBUGLOG

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CubeController : MonoBehaviour
{
    [SerializeField] private InputActionReference _move;
    [SerializeField] private float _rollSpeed = 5;
    
    [Header("Grid Snapping")]
    [SerializeField] private float _snapSpeed = .3f;

    [Tooltip("Transform for syncing cube and player position on switch.")]
    public Transform PlayerCalibratorTransform;

    [Header("Ground Check")]
    [SerializeField] private LayerMask _groundLayerMask;
    
    public Rigidbody Rigidbody;
    private List<Vector3> _directions = new List<Vector3>();
    private Vector3 _newdirection;
    private Camera _cam;

    private GroundCheck _groundCheck { get; set; }
    private Grid _grid;

    public float FallTimer { get; set; }
    [field: SerializeField] public bool IsFalling { get; set; }
    
    private float _horizontal;
    private float _vertical;
    private bool _isMoving;
    private float _weight;
    private bool _touchingGround;

    private void OnEnable()
    {
        _move.action.Enable();
        SnapToGrid();
    }

    private void OnValidate()
    {
        _cam = Camera.main;
        Rigidbody = GetComponent<Rigidbody>();
    }

    private void Awake()
    {
        _grid = FindObjectOfType<Grid>();
    }

    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody>();
        _cam = Camera.main;
        
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

        if (_horizontal <= -0.1f) Tumble(-_cam.transform.right);
        else if (_horizontal >= 0.1f) Tumble(_cam.transform.right);
        else if (_vertical >= 0.1f) Tumble(_cam.transform.forward);
        else if (_vertical <= -0.1f) Tumble(-_cam.transform.forward);

        if (IsFalling) FallTimer += Time.deltaTime;
        else FallTimer = 0;
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
        PlayTumbleSound();
        _isMoving = false;
    #if UNITY_EDITOR && DEBUGLOG 
        Debug.Log("Roll took " + (Time.time - currTime) + "seconds");
    #endif
    }

    private void PlayTumbleSound()
    {
        //Implement sound event here
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
        Gizmos.DrawRay(_cam.transform.position, _cam.transform.forward * 10);
    }
}