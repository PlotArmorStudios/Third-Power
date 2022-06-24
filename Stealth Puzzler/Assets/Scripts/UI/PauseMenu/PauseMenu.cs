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
    public static PlayerInput PlayerInput;

    public static bool IsPaused { get; private set; }

    void Awake()
    {
        IsPaused = false;
        PlayerInput = GetComponent<PlayerInput>();
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

    public void Update()
    {
#if DebugActionMap
        Debug.Log("Current action map: " + PlayerInput.currentActionMap);
        Debug.Log("Current control scheme: " + PlayerInput.currentControlScheme);
#endif
    }

    private void PauseGame(InputAction.CallbackContext obj)
    {
        if (obj.started)
        {
            Time.timeScale = 0;
            _pauseMenu.SetActive(true);
            PlayerInput.SwitchCurrentActionMap("UI");
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
        PlayerInput.SwitchCurrentActionMap("Player");
        IsPaused = false;
    }

    public void Save()
    {
        GameManager.Instance.SaveGame();
    }
}