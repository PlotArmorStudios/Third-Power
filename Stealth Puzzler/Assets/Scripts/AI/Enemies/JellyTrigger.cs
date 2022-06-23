using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JellyTrigger : MonoBehaviour
{
    [SerializeField] private Controller _playerCube;
    [SerializeField] private Controller _playerHumanoid;
    [SerializeField] private InputActionReference _move;
    [SerializeField] private InputActionReference _switchController;
    private bool _isInvincible = false;
    [SerializeField] private float _invicibilityTimeInitial = 1f;
    [SerializeField] private float _invicibilityTimeLeft;

    [SerializeField] private float _jellyUpwardForce = 7f;

    private void Start()
    {
        _invicibilityTimeLeft = 0;
        _playerCube = FindObjectOfType<CubeController>(true);
        _playerHumanoid = FindObjectOfType<PlayerController>(true);
    }
    private void Update()
    {
        InvicibilityCheck();
    }

    private void InvicibilityCheck()
    {
        if (_isInvincible)
        {
            _invicibilityTimeLeft -= Time.deltaTime;
        }
        if (_invicibilityTimeLeft < 0)
        {
            _isInvincible = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    { 
        if (GameObject.ReferenceEquals(other.gameObject, _playerCube.gameObject))
        {
            _move.action.Disable();
            _invicibilityTimeLeft = _invicibilityTimeInitial;
        }
        else if (GameObject.ReferenceEquals(other.gameObject, _playerHumanoid.gameObject) && _invicibilityTimeLeft <= 0)
        {
            _playerHumanoid.GetComponent<Health>().TakeHit();
            print("player hit");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_switchController.action.triggered)
        {
            _isInvincible = true;
            _playerHumanoid.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * _jellyUpwardForce, ForceMode.Impulse);
        }
    }
}
