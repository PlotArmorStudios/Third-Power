using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Drop : MonoBehaviour
{
    [SerializeField] private InputActionReference _drop;
    [SerializeField] private float _dropSpeed;
    [SerializeField] private float _maxDropRate;

    private CubeController _cubeController;
    private ParticleController _particleController;

    private float _dropTime;
    private bool _dropping;
    private Rigidbody _rigidbody;

    private void OnEnable()
    {
        _drop.action.Enable();
    }

    private void OnDisable()
    {
        _drop.action.Disable();
    }

    private void Start()
    {
        _cubeController = GetComponent<CubeController>();
        _particleController = GetComponent<ParticleController>();
        _rigidbody = _cubeController.Rigidbody;
    }

    private void Update()
    {
        if (_drop.action.triggered && !_dropping)
            AerialDrop();

        if (_dropping)
        {
            var dropRate = _dropSpeed + _dropTime * _dropTime;
            if (dropRate > _maxDropRate)
                dropRate = _maxDropRate;
            _rigidbody.velocity = new Vector3(_rigidbody.velocity.x, _rigidbody.velocity.y - dropRate,
                _rigidbody.velocity.z);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!_dropping) return;
        _particleController.StopDropVFX();
        _particleController.PlayDropImpactVFX(transform.position);
        PlayDropImpactSound();
        _dropping = false;
        var crushableObject = collision.gameObject.GetComponent<Crushable>();
        if (!crushableObject) return;
        crushableObject.Crush();
    }


    public void AerialDrop()
    {
        if (!_cubeController.IsTouchingGround())
        {
            StopCoroutine(PerformDrop());
            StartCoroutine(PerformDrop());
        }
    }

    private IEnumerator PerformDrop()
    {
        _cubeController.Rigidbody.velocity = Vector3.zero;
        yield return new WaitForSeconds(.4f);
        _dropping = true;
        _particleController.PlayDropVFX(transform.position);
        PlayDropSound();
    }

    private void PlayDropSound()
    {
        //Implement drop sound here
    }

    private void PlayDropImpactSound()
    {
        //Implement drop impact sound here
    }
}
