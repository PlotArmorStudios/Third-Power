using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ToggleOnPause : MonoBehaviour
{
    [SerializeField] private GameObject _toggleObject;

    [SerializeField] private bool _toggleOnOnResume = true;

    private void OnEnable()
    {
        PauseMenu.OnResume += ToggleOn;
        PauseMenu.OnPause += ToggleOff;
    }

    private void OnDisable()
    {
        PauseMenu.OnResume -= ToggleOn;
        PauseMenu.OnPause -= ToggleOff;
    }

    private void ToggleOn()
    {
        if (_toggleOnOnResume)
            _toggleObject.SetActive(true);
    }

    private void ToggleOff()
    {
        _toggleObject.SetActive(false);
    }
}