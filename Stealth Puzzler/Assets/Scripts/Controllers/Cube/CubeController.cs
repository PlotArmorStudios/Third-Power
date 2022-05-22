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

    public Transform PlayerCalibratorTransform;

    private Rigidbody _rigidbody;
    private List<Vector3> _directions = new List<Vector3>();
    private Vector3 _newdirection;
    private Camera _cam;
    private Grid _grid;

    private float _horizontal;
    private float _vertical;
    private bool _isMoving;

    private void OnEnable()
    {
        _move.action.Enable();
        SnapToGrid();
    }

    private void OnValidate()
    {
        _cam = Camera.main;
    }

    private void Awake()
    {
        _grid = FindObjectOfType<Grid>();
    }

    private void Start()
    {
        _cam = Camera.main;
        _rigidbody = GetComponent<Rigidbody>();
        
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
    }

    private void Tumble(Vector3 dir)
    {
        float largestDot = 0;
        int closestAxis = 0;

        for (int a = 0; a < _directions.Count; a++)
        {
            var dot = Vector3.Dot(dir, _directions[a]);
            if (dot > largestDot)
            {
                largestDot = dot;
                closestAxis = a;
            }
        }

        _newdirection = _directions[closestAxis];
        var anchor = transform.position + (Vector3.down + _newdirection) * 0.5f;
        var axis = Vector3.Cross(Vector3.up, _newdirection);
        StartCoroutine(Roll(anchor, axis));
    }

    private void ReadInput()
    {
        _horizontal = _move.action.ReadValue<Vector2>().x;
        _vertical = _move.action.ReadValue<Vector2>().y;
    }

    // private IEnumerator Roll(Vector3 anchor, Vector3 axis)
    // {
    // #if UNITY_EDITOR 
    //     float currTime = Time.time; 
    // #endif
    //     _isMoving = true;
    //     float deltaTime = Time.deltaTime;
    //     for (var i = 0; i < 90 / (_rollSpeed * Time.deltaTime); i++)
    //     {
    //         transform.RotateAround(anchor, axis, _rollSpeed * Time.deltaTime);
    //         yield return new WaitForSeconds(0.01f);
    //     }

    //     SnapToGrid();
    //     _isMoving = false;
    // #if UNITY_EDITOR 
    //     Debug.Log("Roll took " + (Time.time - currTime) + "seconds");
    // #endif
    // }

    private IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
    #if UNITY_EDITOR 
        float currTime = Time.time; 
    #endif
        _isMoving = true;
        
        Quaternion currRotation = transform.rotation;
        Vector3 currPosition = transform.position;
        transform.RotateAround(anchor, axis, 90);
        Quaternion targetRotation = transform.rotation;
        Vector3 targetPosition = transform.position;
        transform.position = currPosition;
        transform.rotation = currRotation;
        float lerpTime = 0.0f;

        while (transform.rotation != targetRotation)
        {
            transform.rotation = Quaternion.Lerp(currRotation, targetRotation, lerpTime * _rollSpeed);
            transform.position = Vector3.Lerp(currPosition, targetPosition, lerpTime * _rollSpeed);
            lerpTime += Time.deltaTime;
            Debug.Log("Cheese gromit");
            yield return null;
        }

        SnapToGrid();
        _isMoving = false;
    #if UNITY_EDITOR 
        Debug.Log("Roll took " + (Time.time - currTime) + "seconds");
    #endif
    }

    private void SnapToGrid()
    {
        Vector3 snappedPostion = _grid.GetNearestPointOnGrid(transform.position);
        snappedPostion.y = transform.position.y;
        transform.position = Vector3.Lerp(transform.position, snappedPostion, _snapSpeed);
        Debug.Log("Snapped to " + _grid.GetNearestPointOnGrid(transform.position));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _newdirection * 5);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(_cam.transform.position, _cam.transform.forward * 10);
    }
}