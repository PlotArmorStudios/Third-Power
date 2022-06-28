using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAltar : MonoBehaviour
{
    [SerializeField] private AltarInput _altar;
    [SerializeField] private Camera _uiCam;
    
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
