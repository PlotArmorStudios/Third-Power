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

    private void Awake()
    {
        _vCam = GetComponent<CinemachineFreeLook>();
    }

    private void Start()
    {
        _focalPoints = FindObjectOfType<ControllerManager>().FocalPoints;
        _vCam.Follow = _focalPoints[0];
        _vCam.LookAt = _focalPoints[0];
    }

    private void HandleSwitchFocalPoint(int focalPoint)
    {
        _vCam.Follow = _focalPoints[focalPoint - 1];
        _vCam.LookAt = _focalPoints[focalPoint - 1];
    }
}