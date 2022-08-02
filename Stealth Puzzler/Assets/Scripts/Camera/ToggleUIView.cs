using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleUIView : MonoBehaviour
{
    [SerializeField] private LayerMask _viewUIMask;
    [SerializeField] private LayerMask _viewNoUIMask;
    [SerializeField] private LayerMask _cutsceneView;

    public void SwitchOnUI()
    {
        Camera.main.cullingMask = _viewUIMask;
    }

    public void SwitchOffUI()
    {
        Camera.main.cullingMask = _viewNoUIMask;
    }

    public void InitiateCutsceneView()
    {
        Camera.main.cullingMask = _cutsceneView;
    }
}
