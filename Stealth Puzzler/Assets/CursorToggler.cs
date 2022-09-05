using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class CursorToggler : MonoBehaviour
{
    public static event Action OnCursorToggle;
    public Toggle ToggleState;

    public static CursorToggler Instance;

    public bool ToggleCursor;

    private void Awake() => Instance = this;

    private void OnEnable()
    {
        ToggleState.isOn = GameManager.Instance.CursorLockToggle;
        HandleToggleCursor();
    }

    public void HandleToggleCursor()
    {
        Instance.ToggleCursor = ToggleState.isOn;
        OnCursorToggle?.Invoke();
    }
}
