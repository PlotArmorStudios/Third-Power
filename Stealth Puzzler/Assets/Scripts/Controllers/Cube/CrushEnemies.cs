using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CrushEnemies : MonoBehaviour
{
    [SerializeField] private InputActionReference _drop;
    [SerializeField] private float _dropSpeed;
    [SerializeField] private float _maxDropRate;

    private CubeController _cubeController;

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
        _rigidbody = _cubeController.Rigidbody;
    }

    private void Update()
    {
        if (!_cubeController.IsTouchingGround())
            if (_drop.action.triggered)
                _dropping = true;

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
        _dropping = false;
        var crushableObject = collision.gameObject.GetComponent<Crushable>();
        if (!crushableObject) return;
        crushableObject.Crush();
    }
}