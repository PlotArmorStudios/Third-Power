using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject _pauseMenu;
    [SerializeField] private InputActionReference _pause;
    [SerializeField] private InputActionReference _resume;
    [SerializeField] private static PlayerInput _playerInput;
    public static bool IsPaused { get; private set; }
    // Start is called before the first frame update
    void Awake()
    {
        IsPaused = false;
        _playerInput = GetComponent<PlayerInput>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnEnable()
    {
        _pause.action.started += PauseGame;
        _resume.action.started += ResumeGame;
    }
    private void OnDisable()
    {
        _pause.action.started -= PauseGame;
        _resume.action.started -= ResumeGame;
    }
    private void PauseGame(InputAction.CallbackContext obj)
    {
        if (obj.started)
        {
            Time.timeScale = 0;
            _pauseMenu.SetActive(true);
            _playerInput.SwitchCurrentActionMap("UI");
            IsPaused = true;
        }
    }

    private void ResumeGame(InputAction.CallbackContext obj)
    {
        if (obj.started)
        {
            Resume();
        }
    }

    public void Resume()
    {
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
        _playerInput.SwitchCurrentActionMap("Player");
        IsPaused = false;
    }
}
