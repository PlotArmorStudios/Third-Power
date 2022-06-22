using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AltarInput : MonoBehaviour
{
    [SerializeField] private GameObject _altarUI;
    
    public InputActionReference Interact;

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
    }
}