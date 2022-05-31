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
    [SerializeField] private float _toggleDelay = 2f;

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
                break;
            
            case ToggleObjectState.Inactive: 
                break;
        }
        if (_currentToggleTime >= _toggleDelay)
        {
            ToggleObject();
            _currentToggleTime = 0;
        }
    }

    private void ToggleObject()
    { 
        _object.SetActive(!_object.activeSelf);
    }
}
