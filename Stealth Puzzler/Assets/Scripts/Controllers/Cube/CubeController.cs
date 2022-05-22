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

    private IEnumerator Roll(Vector3 anchor, Vector3 axis)
    {
        _isMoving = true;
        for (var i = 0; i < 90 / _rollSpeed; i++)
        {
            transform.RotateAround(anchor, axis, _rollSpeed);
            yield return new WaitForSeconds(0.01f);
        }

        SnapToGrid();
        _isMoving = false;
    }

    private void SnapToGrid()
    {
        Vector3 snappedPostion = _grid.GetNearestPointOnGrid(transform.position);
        snappedPostion.y = transform.position.y;
        transform.position = Vector3.Lerp(transform.position, snappedPostion, _snapSpeed);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, _newdirection * 5);

        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(_cam.transform.position, _cam.transform.forward * 10);
    }
}