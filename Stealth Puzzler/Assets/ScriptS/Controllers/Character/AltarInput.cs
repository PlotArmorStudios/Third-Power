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
    
    public InputActionReference Interact;
    private UIAltar _altarUI;

    private RectTransform _altarRectTransform;

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
        var altarUI = Instantiate(_altarUIObject, transform.position, _altarUIObject.transform.rotation);
        altarUI.transform.parent = transform;
        var rotation = new Quaternion(60, 0, 0, 100);
        altarUI.GetComponent<RectTransform>().rotation = rotation;
        altarUI.SetActive(false);
        _altarUI = altarUI.GetComponent<UIAltar>();
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
        yield return new WaitForSeconds(_timeToActivateUI);
        _altarUI.gameObject.SetActive(true);
    }
}