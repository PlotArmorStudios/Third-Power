using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class CursorLock : MonoBehaviour
{
    [SerializeField] private InputActionReference _cursorLock;

    private List<AltarInput> _altars;

    private void OnEnable()
    {
        _cursorLock.action.Enable();
        _altars = FindObjectsOfType<AltarInput>().ToList();

        foreach (var altar in _altars)
        {
            altar.OnActivateCamInput += HandleEnableCursorLock;
            altar.OnDeactivateCamInput += HandleDisableCursorLock;
        }
    }

    private void OnDisable()
    {
        foreach (var altar in _altars)
        {
            altar.OnActivateCamInput -= HandleEnableCursorLock;
            altar.OnDeactivateCamInput -= HandleDisableCursorLock;
        }
    }

    private void HandleDisableCursorLock()
    {
        Cursor.lockState = CursorLockMode.None;
        _cursorLock.action.Disable();
    }

    private void HandleEnableCursorLock()
    {
        _cursorLock.action.Enable();
    }

    private void Update()
    {
        if (PauseMenu.IsPaused)
        {
            Cursor.lockState = CursorLockMode.None;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
            Cursor.lockState = CursorLockMode.None;
        if (_cursorLock.action.triggered)
        {
            if (CursorToggler.Instance != null && CursorToggler.Instance.ToggleCursor)
                Cursor.lockState = CursorLockMode.Locked;
        }
    }
}