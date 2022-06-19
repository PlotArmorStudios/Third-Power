using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonLever : Obstacle
{
    [SerializeField] private bool _isActive = true;
    [SerializeField] private UnityEvent _pressEvent;
    
    private Animator _animator;
    private bool _isPressed;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        if (GameManager.Instance.ObstacleBooleans[_obstacleID])
        {
            _isPressed = true;   
            _isActive = false;
        }
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
        _isPressed = true;

        if (GameManager.Instance.ObstacleBooleans.ContainsKey(_obstacleID)) return;
        GameManager.Instance.AddObstacleBoolean(_obstacleID, _isPressed);

        PlayButtonPressSound();
    }

    private void PlayButtonPressSound()
    {
        //Implement button press sound here
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