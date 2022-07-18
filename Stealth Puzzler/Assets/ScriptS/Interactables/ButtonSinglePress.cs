using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonSinglePress : Obstacle
{
    [SerializeField] private bool _isActive = true;
    [SerializeField] private UnityEvent _pressEvent;
    
    private Animator _animator;
    private bool _isPressed;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        if (GameManager.Instance.ObstacleBooleans.ContainsKey(_obstacleID))
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
        if (_isPressed) return;
        
        var cube = other.GetComponentInChildren<CubeController>();

        if (!cube) return;
        _animator.SetBool("Pressed", true);

        if (!_isActive) return;
        _pressEvent?.Invoke();
        _isPressed = true;

        if (GameManager.Instance.ObstacleBooleans.ContainsKey(_obstacleID)) return;
        GameManager.Instance.AddObstacleBoolean(_obstacleID, _isPressed);

        PlayButtonPressDownSound();
    }

    private void PlayButtonPressDownSound()
    {
        AkSoundEngine.PostEvent("Play_puzzle_button_and_door_open", gameObject);
    }
}