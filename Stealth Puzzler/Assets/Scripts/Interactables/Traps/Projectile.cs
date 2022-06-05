using System;
using System.Numerics;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _maxRayDistance = 1f;
    public float Speed;
    private Rigidbody _rigidbody;

    private LayerMask _reflectLayerMask;
    private Vector3 _inputDirection;
    private Ray _ray;
    private bool _isBouncing;
    private float _timeActive;

    private void OnValidate()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _timeActive = 0;
    }

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.velocity = _rigidbody.transform.forward * Speed;
    }

    private void Update()
    {
        _timeActive += Time.deltaTime;
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
        //Reflector reflector = other.gameObject.GetComponent<Faces>().GetReflector();
        Reflector reflector = other.gameObject.GetComponentInChildren<Reflector>();
        var enemy = other.gameObject.GetComponent<EnemyAi>();
        var player = other.gameObject.GetComponent<PlayerController>();
        
        Debug.Log(other.gameObject);
        
        if (reflector)
        {
            var wallNormal = other.contacts[0].normal;
            var bounceDirection = Vector3.Reflect(_inputDirection, wallNormal);
            _rigidbody.velocity = bounceDirection * Speed;
            _rigidbody.transform.rotation = Quaternion.LookRotation(bounceDirection);
        }
        else if (enemy)
        {
            enemy.GetComponent<Health>().TakeHit();
            gameObject.SetActive(false);
        }
        else if (player)
        {
            player.GetComponent<Health>().TakeHit();
            gameObject.SetActive(false);
        }
        else
        {
            if (_timeActive < .8f) return;
            gameObject.SetActive(false);
        }
    }
}