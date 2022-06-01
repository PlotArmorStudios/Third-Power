using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ToggleObjectState
{
    Active,
    Inactive
}

public class ReappearingObject : MonoBehaviour
{
    [SerializeField] private GameObject _object;
    [SerializeField] private float _toggleOffDelay = 2f;
    [SerializeField] private float _toggleOnDelay = 2f;

    public bool IsActive;
    private float _currentToggleTime;

    private ToggleObjectState _toggleObjectState = ToggleObjectState.Inactive;
    
    private void Update()
    {
        if (!IsActive) return;
        _currentToggleTime += Time.deltaTime;
        switch (_toggleObjectState)
        {
            case ToggleObjectState.Active:
                if (_currentToggleTime >= _toggleOffDelay)
                {
                    ToggleObject();
                    _toggleObjectState = ToggleObjectState.Inactive;
                    _currentToggleTime = 0;
                }
                break;
            
            case ToggleObjectState.Inactive:
                if (_currentToggleTime >= _toggleOnDelay)
                {
                    ToggleObject();
                    _toggleObjectState = ToggleObjectState.Active;
                    _currentToggleTime = 0;
                }
                break;
        }
        
    }

    public void Activate()
    {
        IsActive = true;
    }

    public void Deactivate()
    {
        IsActive = false;
    }

    private void ToggleObject()
    { 
        _object.SetActive(!_object.activeSelf);
    }
}
