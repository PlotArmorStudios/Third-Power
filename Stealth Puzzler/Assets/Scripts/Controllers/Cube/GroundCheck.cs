using System;
using System.Collections;
using UnityEngine;

public class GroundCheck : MonoBehaviour
{
    [SerializeField] LayerMask _groundLayerMask;
    [SerializeField] Transform _feet;

    private Animator _animator;
    bool _isGrounded;
    public bool IsGrounded => _isGrounded;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }
    private void FixedUpdate()
    {
        UpdateIsGrounded();
    }

    public bool UpdateIsGrounded()
    {
        _isGrounded = Physics.CheckSphere(_feet.position, 1f, _groundLayerMask);
        return _isGrounded;
    }
}