using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonLever : MonoBehaviour
{
    [SerializeField] private bool _isActive = true;
    [SerializeField] private UnityEvent _pressEvent;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    public void Activate()
    {
        _isActive = true;
    }

    public void Deactivate()
    {
        _isActive = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        var cube = other.GetComponentInChildren<CubeController>();
        if (!cube) return;
        _animator.SetBool("Pressed", true);
        if (!_isActive) return;
        _pressEvent?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        var cube = other.GetComponentInChildren<CubeController>();
        var player = other.GetComponentInChildren<PlayerController>();
        if (!_isActive) return;
        if (cube || player)
            _animator.SetBool("Pressed", false);
    }
}