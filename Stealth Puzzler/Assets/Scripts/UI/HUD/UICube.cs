using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UICube : MonoBehaviour
{
    [SerializeField] private RectTransform _UICanvas;
    private void OnEnable()
    {
        ControllerManager.OnSwitchToCube += ShowCubeUI;
        ControllerManager.OnSwitchToHuman += HideCubeUI;
    }
    private void OnDisable()
    {
        ControllerManager.OnSwitchToCube -= ShowCubeUI;
        ControllerManager.OnSwitchToHuman -= HideCubeUI;
    }
    // Start is called before the first frame update
    private void ShowCubeUI()
    {
        LeanTween.alpha(_UICanvas, 1f, 0.5f).setFrom(0);
    }
    private void HideCubeUI()
    {
        LeanTween.alpha(_UICanvas, 0, 0.5f);
    }
}