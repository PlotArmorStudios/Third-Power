using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AltarOverlayCamera : MonoBehaviour
{
    private Camera _mainCamera;
    private Camera _thisCam;
    
    private UniversalAdditionalCameraData _camData;
    
    private void OnEnable()
    {
        _mainCamera = Camera.main;
        _thisCam = GetComponent<Camera>();
        _camData = _mainCamera.GetUniversalAdditionalCameraData();
        _camData.cameraStack.Add(_thisCam);
    }

    private void OnDisable()
    {
        _camData.cameraStack.Remove(_thisCam);
    }
}
