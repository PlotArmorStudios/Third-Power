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
        InputSystem.onEvent +=
            (eventPtr, device) =>
            {
            // Ignore anything that isn't a state event.
                //if (!eventPtr.IsA<StateEvent>() && !eventPtr.IsA<DeltaStateEvent>())
                    //return;
                var gamepad = device as Gamepad;
                var keyboard = device as Keyboard;
                var mouse = device as Mouse;
                
                
                if (gamepad != null && _lastInput == gamepad)
                {
                    print("gamepad");
                    _lastInput = gamepad;
                    _keyboardControls.SetActive(false);
                    _gamepadControls.SetActive(true);
                }
                if ((keyboard != null || mouse != null) && (_lastInput == keyboard || _lastInput == mouse))
                {
                    print("keyboard and mouse");
                    _lastInput = keyboard;
                    _gamepadControls.SetActive(false);
                    _keyboardControls.SetActive(true);
                }
            };
    }
    
}
