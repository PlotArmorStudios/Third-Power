using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.InputSystem.Layouts;

public class SwitchDeviceUI : MonoBehaviour
{
    [SerializeField] private GameObject _keyboardControls;
    [SerializeField] private GameObject _gamepadControls;
    private InputDevice _lastInput;

    private void OnEnable()
    {
        InputSystem.onEvent += RegisterDevice;
    }

    private void OnDisable()
    {
        InputSystem.onEvent -= RegisterDevice;
    }

    private void RegisterDevice(InputEventPtr eventPtr, InputDevice device)
    {
        // Ignore anything that isn't a state event.
        if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
            return;
        var gamepad = device as Gamepad;
        var keyboard = device as Keyboard;
        var mouse = device as Mouse;

        if (gamepad != null)
        {
            _keyboardControls.SetActive(false);
            _gamepadControls.SetActive(true);
        }

        if ((keyboard != null || mouse != null))
        {
            _gamepadControls.SetActive(false);
            _keyboardControls.SetActive(true);
        }
    }
}