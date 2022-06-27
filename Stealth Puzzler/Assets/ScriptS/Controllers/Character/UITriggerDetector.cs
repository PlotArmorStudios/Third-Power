using System;
using UnityEngine;

public class UITriggerDetector : MonoBehaviour
{
    [SerializeField] private GameObject _uiObject;

    private void OnTriggerExit(Collider other)
    {
        _uiObject.SetActive(false);
    }

    private void OnTriggerStay(Collider other)
    {
        var playerForward = other.gameObject.transform.forward;
        var triggerForward = gameObject.transform.forward;
        var dot = Vector3.Dot(playerForward, triggerForward);
        
        Debug.Log("Dot: " + dot);
        
        if(dot > 0) _uiObject.SetActive(true);
        else _uiObject.SetActive(false);
        
    }
}