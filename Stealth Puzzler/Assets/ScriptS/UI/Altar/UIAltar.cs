using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UIAltar : MonoBehaviour
{
    [SerializeField] private AltarInput _altar;
    [SerializeField] private Camera _uiCam;
    [SerializeField] private InputActionReference _pause;
    [SerializeField] private InputActionReference _resume;
    private void OnEnable()
    {
        _altar.OnActivateUI += HandleActivateCamera;
        _altar.OnDeactivateUI += HandleDeactivateCamera;
        _uiCam.gameObject.SetActive(true);
    }

    private void OnDisable()
    {
        _altar.OnActivateUI -= HandleActivateCamera;
        _altar.OnDeactivateUI -= HandleDeactivateCamera;
        _pause.action.Enable();
        _resume.action.Enable();
    }
    
    private void HandleActivateCamera()
    {
        _uiCam.gameObject.SetActive(true);
        
    }

    private void HandleDeactivateCamera()
    {
        _uiCam.gameObject.SetActive(false);
    }
}
