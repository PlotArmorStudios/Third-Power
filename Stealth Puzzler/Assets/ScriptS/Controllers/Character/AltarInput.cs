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

    [SerializeField] private LayerMask _viewUIMask;
    [SerializeField] private LayerMask _noViewUIMask;
    
    public InputActionReference Interact;

    private RectTransform _altarRectTransform;

    private Camera _mainCamera;

    private void OnEnable()
    {
        Interact.action.started += OnInteract;
    }

    private void OnDisable()
    {
        Interact.action.started -= OnInteract;
    }

    private void OnInteract(InputAction.CallbackContext obj)
    {
        Debug.Log("Interacting");
        StartCoroutine(ActivateAltarUI());
        DeactivateInteractInput();
    }

    private void DeactivateInteractInput()
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
        _altarTriggerBox.SetActive(false);
        
        yield return new WaitForSeconds(_timeToActivateUI);
        _altarUI.gameObject.SetActive(true);
    }

    public void DeactivateAltarUi()
    {
        OnDeactivateUI?.Invoke();
        _mainCamera = Camera.main;
        _mainCamera.cullingMask = _viewUIMask;
        
        PauseMenu.PlayerInput.SwitchCurrentActionMap("Player");
        _altarTriggerBox.SetActive(true);
        StartCoroutine(DeactivateUI());
    }

    private IEnumerator DeactivateUI()
    {
        yield return new WaitForSeconds(_timeToDeactivateUI);
        _altarUI.gameObject.SetActive(false);
        OnActivateCamInput?.Invoke();
    }
}