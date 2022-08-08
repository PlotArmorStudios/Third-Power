using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class FocalPointManager : MonoBehaviour
{
    [SerializeField] private List<Transform> _focalPoints;

    private CinemachineVirtualCameraBase _vCam;

    private void OnEnable() => ControllerManager.OnSwitchFocalPoints += HandleSwitchFocalPoint;
    private void OnDisable() => ControllerManager.OnSwitchFocalPoints -= HandleSwitchFocalPoint;

    private void Awake()
    {
        _vCam = GetComponent<CinemachineVirtualCameraBase>();
    }

    private IEnumerator Start()
    {
        yield return new WaitForSeconds(.5f);
        
        _focalPoints = ControllerManager.Instance.FocalPoints;
        _vCam.Follow = _focalPoints[0];
        _vCam.LookAt = _focalPoints[0];
    }

    public void InitializeFocalPoints(ControllerManager controllerManager)
    {
        _focalPoints = controllerManager.FocalPoints;
        _vCam.Follow = _focalPoints[0];
        _vCam.LookAt = _focalPoints[0];
    }

    private void HandleSwitchFocalPoint(int focalPoint)
    {
        _vCam.Follow = _focalPoints[focalPoint - 1];
        _vCam.LookAt = _focalPoints[focalPoint - 1];
    }
}