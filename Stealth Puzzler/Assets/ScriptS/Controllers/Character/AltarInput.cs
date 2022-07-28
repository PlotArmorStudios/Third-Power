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

    private RectTransform _altarRectTransform;

    private Camera _mainCamera;

    private void OnDisable()
    {
        Interact.action.started -= OnInteract;
    }

    private void OnInteract(InputAction.CallbackContext obj)
    {
        Debug.Log("Interacting");
        PlayAltarEngageSound();
        StartCoroutine(ActivateAltarUI());
        DeactivateInteractInput();
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
        OnDeactivateUI?.Invoke();
        _mainCamera = Camera.main;
        _mainCamera.cullingMask = _viewUIMask;
        
        PauseMenu.PlayerInput.SwitchCurrentActionMap("Player");
        _boxVolume.SetActive(false);
        _altarTriggerBox.SetActive(true);
        PlayAltarDisengageSound();
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
    }

    private void PlayAltarDisengageSound()
    {
        AkSoundEngine.PostEvent("Play_Podium_Deactivate", gameObject);
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
