using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AltarInput : MonoBehaviour
{
    public event Action OnActivateUI;
    public event Action OnDeactivateUI;
    
    [SerializeField] private GameObject _altarUIObject;
    
    public InputActionReference Interact;
    private UIAltar _altarUI;
    private void OnEnable()
    {
        Interact.action.started += OnInteract;
    }

    private void OnDisable()
    {
        Interact.action.started -= OnInteract;
    }

    private void Start()
    {
        var altarUI = Instantiate(_altarUIObject, transform.position, Quaternion.identity);
        altarUI.transform.parent = transform;
        altarUI.SetActive(false);
        _altarUI = altarUI.GetComponent<UIAltar>();
    }

    private void OnInteract(InputAction.CallbackContext obj)
    {
        Debug.Log("Interacting");
        ActivateAltarUI();
        DeactivateInteractInput();
    }

    private void DeactivateInteractInput()
    {
        Interact.action.Disable();
    }

    private void ActivateAltarUI()
    {
        _altarUI.gameObject.SetActive(true);
        OnActivateUI?.Invoke();
    }
}