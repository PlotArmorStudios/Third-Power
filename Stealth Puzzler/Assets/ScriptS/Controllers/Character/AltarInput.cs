using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class AltarInput : MonoBehaviour
{
    public InputActionReference Interact;

    private void Update()
    {
        if (Interact.action.triggered)
        {
            Debug.Log("Interacting");
        }
    }
}