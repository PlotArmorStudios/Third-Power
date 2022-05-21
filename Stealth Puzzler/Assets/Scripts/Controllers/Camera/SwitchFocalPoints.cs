using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SwitchFocalPoints : MonoBehaviour
{
    [SerializeField] private List<Transform> _focalPoints;

    private CinemachineFreeLook _vCam;

    private void OnEnable() => ControllerManager.OnSwitchFocalPoints += HandleSwitchFocalPoint;
    private void OnDisable() => ControllerManager.OnSwitchFocalPoints -= HandleSwitchFocalPoint;

    private void Start()
    {
        _vCam = GetComponent<CinemachineFreeLook>();
    }

    private void HandleSwitchFocalPoint(int focalPoint)
    {
        if (_vCam)
        {
            _vCam.Follow = _focalPoints[focalPoint - 1];
            _vCam.LookAt = _focalPoints[focalPoint - 1];
        }
    }
}