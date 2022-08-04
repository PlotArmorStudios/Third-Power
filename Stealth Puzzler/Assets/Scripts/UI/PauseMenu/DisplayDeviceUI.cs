using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class DisplayDeviceUI : MonoBehaviour
{
    [SerializeField] private GameObject _keyboardControls;
    [SerializeField] private GameObject _gamepadControls;
    private void OnEnable()
    {
        InputSystem.onEvent +=
            (eventPtr, device) =>
            {
            // Ignore anything that isn't a state event.
                if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
                    return;
                var gamepad = device as Gamepad;
                var keyboard = device as Keyboard;
                if (gamepad != null)
                {
                    _keyboardControls.SetActive(false);
                    _gamepadControls.SetActive(true);
                }
                if (keyboard != null)
                {
                    _gamepadControls.SetActive(false);
                    _keyboardControls.SetActive(true);
                }
            };
    }
}
