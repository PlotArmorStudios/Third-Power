using System;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Projectile : MonoBehaviour
{
    public float Speed;
    private Rigidbody _rigidbody;

    public int _maxReflectionCount = 5;
    public float _maxStepDistance = 200;
    private LayerMask _reflectLayerMask;
    [SerializeField] private float _maxRayDistance = 1f;
    private Vector3 _inputDirection;
    private Ray _ray;

    private void OnValidate()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = _rigidbody.transform.forward * Speed;
    }

    private void FixedUpdate()
    {
        CalculateReflectionRay();
    }

    private void CalculateReflectionRay()
    {
        _ray = new Ray(_rigidbody.transform.position, _rigidbody.transform.forward);
        RaycastHit hit;

        if (Physics.Raycast(_ray, out hit, _maxRayDistance))
        {
            _inputDirection = (hit.point - _rigidbody.transform.position).normalized;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        var wallNormal = other.contacts[0].normal;
        var bounceDirection = Vector3.Reflect(_inputDirection, wallNormal);
        _rigidbody.velocity = bounceDirection * Speed;
        _rigidbody.transform.rotation = Quaternion.Inverse(_rigidbody.transform.rotation);
        //_rigidbody.velocity = _rigidbody.transform.forward * Speed;
    }

    void OnDrawGizmos()
    {
        Handles.color = Color.red;
        Handles.ArrowHandleCap(0, _rigidbody.transform.position + _rigidbody.transform.forward * 0.25f, _rigidbody.transform.rotation, 0.5f, EventType.Repaint);
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_rigidbody.transform.position, 0.25f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawRay(_rigidbody.transform.position, _rigidbody.transform.forward * _maxRayDistance);
        DrawPredictedReflectionPattern(_rigidbody.transform.position + _rigidbody.transform.forward * 0.75f, _rigidbody.transform.forward, _maxReflectionCount);
    }

    private void DrawPredictedReflectionPattern(Vector3 position, Vector3 direction, int reflectionsRemaining)
    {
        if (reflectionsRemaining == 0) return;

        Vector3 startingPosition = position;

        Ray ray = new Ray(position, direction);
        RaycastHit hit;
        
        if (Physics.Raycast(ray, out hit, _maxStepDistance))
        {
            direction = Vector3.Reflect(direction, hit.normal);
            position = hit.point;
        }
        else
        {
            position += direction * _maxStepDistance;
        }

        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(startingPosition, position);

        DrawPredictedReflectionPattern(position, direction, reflectionsRemaining - 1);
    }
}