using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DataToggle : MonoBehaviour
{
    [SerializeField] private Button _loadButton;
    
    private void Awake()
    {
        var data = SaveSystem.LoadGame();
        _loadButton.interactable = data != null;
    }

    private void OnEnable() => SaveSystem.OnDeleteSave += RefreshToggle;
    private void OnDisable() => SaveSystem.OnDeleteSave -= RefreshToggle;

    private void RefreshToggle()
    {
        var data = SaveSystem.LoadGame();
        _loadButton.interactable = data != null;
    }
}