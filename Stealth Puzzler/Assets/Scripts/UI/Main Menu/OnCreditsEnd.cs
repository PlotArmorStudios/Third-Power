using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class OnCreditsEnd : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _creditsMask;
    [SerializeField] private InputActionReference _select;
    private void OnEnable()
    {
        _select.action.started += ExitToMain;
    }
    private void OnDisable()
    {
        _select.action.started -= ExitToMain;
    }

    private void ExitToMain(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            OnCreditsEnding();
        }
    }

    public void OnCreditsEnding()
    {
        _creditsMask.SetActive(false);
        _mainMenu.SetActive(true);
    }
}
