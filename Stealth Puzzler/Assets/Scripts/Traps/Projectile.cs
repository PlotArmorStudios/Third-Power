using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Speed;
    private Rigidbody _rigidbody;
    
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        _rigidbody.velocity = transform.forward * Speed;
    }
}