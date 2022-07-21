using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System;

public class OnCreditsEnd : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _creditsPanel;
    [SerializeField] private InputActionReference _exitToMain;
    [SerializeField] private Button _newGameButton;
    private void OnEnable()
    {
        _exitToMain.action.started += ExitToMain;
    }
    private void OnDisable()
    {
        _exitToMain.action.started -= ExitToMain;
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
        _creditsPanel.SetActive(false);
        _mainMenu.SetActive(true);
    }
}
