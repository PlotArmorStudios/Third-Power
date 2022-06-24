using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class AltarInput : MonoBehaviour
{
    public event Action OnActivateUI;
    public event Action OnDeactivateUI;
    
    [SerializeField] private GameObject _altarUIObject;
    [SerializeField] private float _timeToActivateUI = .5f;
    [SerializeField] private UIAltar _altarUI;
    [SerializeField] private GameObject _altarTriggerBox;
    
    public InputActionReference Interact;

    private RectTransform _altarRectTransform;

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
        PauseMenu.PlayerInput.SwitchCurrentActionMap("UI");
        _altarTriggerBox.SetActive(false);
        yield return new WaitForSeconds(_timeToActivateUI);
        _altarUI.gameObject.SetActive(true);
    }
}