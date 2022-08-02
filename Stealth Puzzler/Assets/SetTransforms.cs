using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class SetTransforms : MonoBehaviour
{
    private CinemachineFreeLook _vCam;
    
    private IEnumerator Start()
    {
        _vCam = GetComponent<CinemachineFreeLook>();
        yield return new WaitForSeconds(2f);
        _vCam.m_Follow = FindObjectOfType<CubeController>(true).gameObject.transform;
        _vCam.m_LookAt = FindObjectOfType<CubeController>(true).gameObject.transform;
    }
}
