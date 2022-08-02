using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SetTransforms : MonoBehaviour
{
    [SerializeField] private bool _setLookAt;
    [SerializeField] private bool _setFollow;

    private CinemachineFreeLook _vCam;

    private IEnumerator Start()
    {
        _vCam = GetComponent<CinemachineFreeLook>();
        yield return new WaitForSeconds(2f);
        if (_setLookAt)
            _vCam.m_LookAt = FindObjectOfType<CubeController>(true).gameObject.transform;
        if (_setFollow)
            _vCam.m_Follow = FindObjectOfType<CubeController>(true).gameObject.transform;
    }

    public void SetLookAt()
    {
        _vCam.m_LookAt = FindObjectOfType<CubeController>(true).gameObject.transform;
    }

    public void SetFollow()
    {
        _vCam.m_Follow = FindObjectOfType<CubeController>(true).gameObject.transform;
    }
}