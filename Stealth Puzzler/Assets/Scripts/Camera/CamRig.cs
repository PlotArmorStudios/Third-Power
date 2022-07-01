using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CamRig : MonoBehaviour
{
    public static CamRig Instance;
    public CinemachineFreeLook VCam;

    private void OnEnable()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void UnsetFollow()
    {
        VCam.Follow = null;
        Debug.Log("Follow is nulled");
    }
}
