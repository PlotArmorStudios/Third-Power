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
    [SerializeField] private InputActionReference _interact;
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
        AkSoundEngine.SetState("Menu", "Menu_Inactive");
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
        if (obj.started && !IsPaused)
        {
            OpenPauseMenu();
        }
    }

    private void OpenPauseMenu()
    {
        Time.timeScale = 0;
        _pauseMenu.SetActive(true);
        PlayerInput.SwitchCurrentActionMap("UI");
        IsPaused = true;
        AkSoundEngine.SetState("Menu", "Menu_Active");
    }

    private void ResumeGame(InputAction.CallbackContext obj)
    {
        if (obj.started && IsPaused)
        {
            ClosePauseMenu();
            AkSoundEngine.SetState("Menu", "Menu_Inactive");
        }
    }

    public void ClosePauseMenu()
    {
        Time.timeScale = 1;
        _pauseMenu.SetActive(false);
        PlayerInput.SwitchCurrentActionMap("Player");
        IsPaused = false;
        AkSoundEngine.SetState("Menu", "Menu_Inactive");
        AkSoundEngine.PostEvent("Play_UI_NormalClick", gameObject);
    }

    public void Save()
    {
        GameManager.Instance.SaveGame();
        AkSoundEngine.PostEvent("Play_UI_NormalClick", gameObject);
    }
}