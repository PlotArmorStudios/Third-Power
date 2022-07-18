using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DoorManager))]
public class MultipleButtonDoor : MonoBehaviour
{
    [SerializeField] private int _requiredButtons = 5;
    private int _currButtons = 0;
    private DoorManager _doorManager;

    private void Awake()
    {
        _doorManager = GetComponent<DoorManager>();
    }

    public void ButtonPressed()
    {
        _currButtons++;
        Debug.Log("PRESSED");
        if (_currButtons == _requiredButtons)
            _doorManager.OpenDoor();
    }
}
