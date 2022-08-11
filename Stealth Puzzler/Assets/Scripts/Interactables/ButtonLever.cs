using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ButtonLever : Obstacle
{
    [SerializeField] private bool _isActive = true;
    [SerializeField] private UnityEvent _pressEvent;
    [SerializeField] private bool _loadFromSave;
    
    private Animator _animator;
    private bool _isPressed;
    private bool _buttonTriggered;

    private void Start()
    {
        _animator = GetComponent<Animator>();

        if (GameManager.Instance.ObstacleBooleans.ContainsKey(_obstacleID) && _loadFromSave)
        {
            _isPressed = true;   
            _isActive = false;
        }
    }

    private void Update()
    {
        if (_buttonTriggered) return;
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
        if (_buttonTriggered) return;
        _animator.SetBool("Pressed", true);
        StartCoroutine(ResetButtonTrigger());

        if (!_isActive) return;
        _pressEvent?.Invoke();
        _isPressed = true;

        if (GameManager.Instance.ObstacleBooleans.ContainsKey(_obstacleID)) return;
        GameManager.Instance.AddObstacleBoolean(_obstacleID, _isPressed);
        PlayButtonPressDownSound();
    }

    private IEnumerator ResetButtonTrigger()
    {
        _buttonTriggered = true;
        yield return new WaitForSeconds(.3f);
        _buttonTriggered = false;
    }

    private void PlayButtonPressDownSound()
    {
        AkSoundEngine.PostEvent("Play_puzzle_button_and_door_open", gameObject);
    }

    private void OnTriggerExit(Collider other)
    {
        var cube = other.GetComponentInChildren<CubeController>();
        var player = other.GetComponentInChildren<PlayerController>();

        if (!_isActive) return;

        if (cube || player)
            _animator.SetBool("Pressed", false);
        else
            return;
        
        PlayButtonPressUpSound();
    }

    private void PlayButtonPressUpSound()
    {
        //Implement button press up sound
    }
}