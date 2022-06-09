using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GoToMainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    private readonly InputAction _anyKeyWait = new InputAction(binding: "/*/<button>", type: InputActionType.Button);
    private void OnEnable()
    {
        _anyKeyWait.performed += DoSomething;
        _anyKeyWait.Enable();
    }
    private void OnDisable()
    {
        _anyKeyWait.Disable();
        _anyKeyWait.performed -= DoSomething;
    }
    private void DoSomething(InputAction.CallbackContext ctx) => AnyKey();
    private void AnyKey()
    {
        gameObject.SetActive(false);
        _mainMenu.SetActive(true);
    }
}
