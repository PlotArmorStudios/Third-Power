using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AltarInput : MonoBehaviour
{
    public event Action OnActivateUI;
    public event Action OnDeactivateUI;
    public event Action OnActivateCamInput;
    public event Action OnDeactivateCamInput;
    
    [SerializeField] private GameObject _altarUIObject;
    [SerializeField] private float _timeToActivateUI = .5f;
    [SerializeField] private float _timeToDeactivateUI = 1f;
    [SerializeField] private UIAltar _altarUI;
    [SerializeField] private GameObject _altarTriggerBox;
    [SerializeField] private GameObject _boxVolume;

    [SerializeField] private LayerMask _viewUIMask;
    [SerializeField] private LayerMask _noViewUIMask;
    
    public InputActionReference Interact;
    [SerializeField] private InputActionReference _pause;
    [SerializeField] private InputActionReference _resume;
    [SerializeField] private InputActionReference _exitAltarPopup;

    private bool IsActive = false;

    private RectTransform _altarRectTransform;

    private Camera _mainCamera;

    private void OnEnable()
    {
        _exitAltarPopup.action.started += CloseAltarUI;
    }

    private void OnDisable()
    {
        Interact.action.started -= OnInteract;
        _exitAltarPopup.action.started -= CloseAltarUI;
    }
    private void CloseAltarUI(InputAction.CallbackContext context)
    {
        if (context.started && IsActive)
        {
            DeactivateAltarUi();
        }
    }

    private void OnInteract(InputAction.CallbackContext obj)
    {
        IsActive = true;
        _pause.action.Disable();
        _resume.action.Disable();
        Debug.Log("Interacting");
        PlayAltarEngageSound();
        DeactivateInteractInput();
        StopAllCoroutines();
        StartCoroutine(ActivateAltarUI());
    }

    public void ActivateInteractInput()
    {
        Interact.action.started += OnInteract;
        Interact.action.Enable();
    }

    public void DeactivateInteractInput()
    {
        Interact.action.Disable();
    }

    private IEnumerator ActivateAltarUI()
    {

        OnActivateUI?.Invoke();
        OnDeactivateCamInput?.Invoke();
        _mainCamera = Camera.main;
        _mainCamera.cullingMask = _noViewUIMask;
        PauseMenu.PlayerInput.SwitchCurrentActionMap("UI");
        _boxVolume.SetActive(true);
        _altarTriggerBox.SetActive(false);
        
        yield return new WaitForSeconds(_timeToActivateUI);
        _altarUI.gameObject.SetActive(true);
        PlayActivateUISound();
        
    }


    public void DeactivateAltarUi()
    {
        IsActive = false;
        OnDeactivateUI?.Invoke();
        _mainCamera = Camera.main;
        _mainCamera.cullingMask = _viewUIMask;
        
        PauseMenu.PlayerInput.SwitchCurrentActionMap("Player");
        _boxVolume.SetActive(false);
        _altarTriggerBox.SetActive(true);
        PlayAltarDisengageSound();
        StopAllCoroutines();
        StartCoroutine(DeactivateUI());
    }


    private IEnumerator DeactivateUI()
    {
        yield return new WaitForSeconds(_timeToDeactivateUI);
        _altarUI.gameObject.SetActive(false);
        OnActivateCamInput?.Invoke();
        PlayDeactivateUISound();
    }

    private void PlayAltarEngageSound()
    {
        AkSoundEngine.PostEvent("Play_Podium_Activate", gameObject);
        AkSoundEngine.SetState("Altar","Activated");
    }

    private void PlayAltarDisengageSound()
    {
        AkSoundEngine.PostEvent("Play_Podium_Deactivate", gameObject);
        AkSoundEngine.SetState("Altar", "Deactivated");
    }
    
    private void PlayActivateUISound()
    {
        //Implement altar ui activate sound
    }
    
    private void PlayDeactivateUISound()
    {
        //Implement altar ui deactivate sound
    }

}
