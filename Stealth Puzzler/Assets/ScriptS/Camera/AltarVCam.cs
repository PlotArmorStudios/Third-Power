using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class AltarVCam : MonoBehaviour
{
    [SerializeField] private AltarInput _altar;

    private CinemachineVirtualCamera _vCam;
    
    private void OnEnable()
    {
        _altar.OnActivateUI += ActivatePriority;
        _altar.OnDeactivateUI += DeactivatePriority;
    }
    
    private void OnDisable()
    {
        _altar.OnActivateUI -= ActivatePriority;
        _altar.OnDeactivateUI -= DeactivatePriority;
    }

    private void Start()
    {
        _vCam = GetComponent<CinemachineVirtualCamera>();
    }

    private void ActivatePriority()
    {
        _vCam.Priority = 15;
    }

    private void DeactivatePriority()
    {
        _vCam.Priority = 0;
    }
}
