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
        _rigidbody.transform.rotation = Quaternion.LookRotation(bounceDirection);
    }
}