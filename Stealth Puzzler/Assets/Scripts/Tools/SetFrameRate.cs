using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetFrameRate : MonoBehaviour
{
#if UNITY_EDITOR

    [SerializeField] private int _frameRate = 60;
    private void Awake() 
    {
        
            QualitySettings.vSyncCount = 0;  // VSync must be disabled
            Application.targetFrameRate = _frameRate;
        
    }

    private void OnValidate() 
    {
        Application.targetFrameRate = _frameRate;
    }
#endif    
}
