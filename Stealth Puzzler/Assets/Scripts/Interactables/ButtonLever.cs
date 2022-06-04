using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonLever : MonoBehaviour
{
    [SerializeField] private UnityEvent _pressEvent;
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        var cube = other.GetComponentInChildren<CubeController>();
        
        if (!cube) return;
        _animator.SetBool("Pressed", true);
        _pressEvent?.Invoke();
    }

    private void OnTriggerExit(Collider other)
    {
        var cube = other.GetComponentInChildren<CubeController>();
        var player = other.GetComponentInChildren<PlayerController>();
        
        if (cube || player)
            _animator.SetBool("Pressed", false);
    }
}