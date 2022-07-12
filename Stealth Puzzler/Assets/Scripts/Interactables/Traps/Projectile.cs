using System;
using System.Numerics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float _infrontValue = .55f;
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

    public void SetSpeed(float speed)
    {
        var rigidBody = GetComponent<Rigidbody>();
        Speed = speed;
         rigidBody.velocity = rigidBody.transform.forward * speed;
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
        var enemy = other.gameObject.GetComponent<WolfAI>();
        var player = other.gameObject.GetComponent<PlayerController>();

        if (reflector)
        {
            var directionToTarget = Vector3.Normalize(transform.position - reflector.transform.position);
            var dot = Vector3.Dot(reflector.transform.forward, directionToTarget);
        
            Debug.Log("Detected reflector. Dot product: " + dot);
            
            if (!(dot > _infrontValue))
            {
                if (_timeActive < .3f) return;
                AkSoundEngine.PostEvent("Play_Eye_Impact", gameObject);
                gameObject.SetActive(false);
                return;
            }
            
            var wallNormal = other.contacts[0].normal;
            var bounceDirection = Vector3.Reflect(_inputDirection, wallNormal);
            _rigidbody.velocity = bounceDirection * Speed;
            _rigidbody.transform.rotation = Quaternion.LookRotation(bounceDirection.normalized);
            PlayReflectSound();
        }
        else if (enemy)
        {
            enemy.GetComponent<Health>().TakeHit();
            gameObject.SetActive(false);
        }
        else if (player)
        {
            player.GetComponent<Health>().TakeHit();
            PlayStabSound();
            gameObject.SetActive(false);
        }
        else
        {
            if (_timeActive < .3f) return;
            AkSoundEngine.PostEvent("Play_Eye_Impact", gameObject);
            gameObject.SetActive(false);
        }
    }

    private void PlayStabSound()
    {
        //Implement damage 'stab' sound
        //The sound will likely cut off immediately because the object will be turned off directly after hitting the player.
        //This will be addressed later
    }

    private void PlayReflectSound()
    {
        AkSoundEngine.PostEvent("Play_Cube_Deflect", gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_rigidbody.transform.position, 1f);
    }
}