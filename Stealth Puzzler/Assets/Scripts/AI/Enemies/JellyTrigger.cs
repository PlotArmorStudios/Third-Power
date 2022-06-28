using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JellyTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _jellyController;
    private bool _jellyCanExpand = true;
    [SerializeField] private float _jellyScaleSize = 1.5f;
    [SerializeField] private float _jellyTimeOfExpansion = 2f;
    [SerializeField] private InputActionReference _move;
    [SerializeField] private InputActionReference _switchController;
    private bool _isInvincible = false;
    [SerializeField] private float _invicibilityTimeInitial = 1f;
    [SerializeField] private float _invicibilityTimeLeft;

    [SerializeField] private float _jellyUpwardForce = 7f;

    private void Start()
    {
        _invicibilityTimeLeft = 0;
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
        
        if (other.gameObject.GetComponent<CubeController>())
        {
            _move.action.Disable();
            _invicibilityTimeLeft = _invicibilityTimeInitial;
            ExpandJelly();
        }
        else if (other.gameObject.GetComponent<PlayerController>() && _invicibilityTimeLeft <= 0)
        {
            print("humanoid hit");
            other.gameObject.GetComponent<Health>().TakeHit();
            ExpandJelly();
        }
    }

    private void ExpandJelly()
    {
        if (_jellyCanExpand)
        {
            _jellyCanExpand = false;
            LeanTween.scale(_jellyController, Vector3.one * _jellyScaleSize, _jellyTimeOfExpansion).setEase(LeanTweenType.easeSpring);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (_switchController.action.triggered)
        {
            _isInvincible = true;
            other.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * _jellyUpwardForce, ForceMode.Impulse);
        }
    }
}
