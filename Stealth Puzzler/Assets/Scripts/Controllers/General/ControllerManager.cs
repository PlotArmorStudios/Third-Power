using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerManager : MonoBehaviour
{
    public static event Action<int> OnSwitchFocalPoints;
    [SerializeField] private InputActionReference _switch;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private CubeController _cubeController;

    [SerializeField] private ActiveController _activeController = ActiveController.Player;

    private void OnEnable()
    {
        _switch.action.Enable();
    }

    private void OnDisable()
    {
        _switch.action.Disable();
    }

    private void Start()
    {
        SwitchControllers();
    }

    private void Update()
    {
        if (_switch.action.triggered)
        {
            SwitchControllers();
        }
    }

    private void SwitchControllers()
    {
        switch (_activeController)
        {
            case ActiveController.Player:
                _cubeController.transform.position = _playerController.CubeCalibratorTransform.position;
                _playerController.gameObject.SetActive(false);
                _cubeController.gameObject.SetActive(true);
                OnSwitchFocalPoints?.Invoke(2);
                _activeController = ActiveController.Cube;
                break;
            case ActiveController.Cube:
                var distance = Vector3.Distance(_playerController.transform.position,
                    _playerController.CubeCalibratorTransform.position);

                _playerController.transform.position = _cubeController.transform.position +
                                                       (Vector3.up * distance);

                _playerController.gameObject.SetActive(true);
                _cubeController.gameObject.SetActive(false);
                OnSwitchFocalPoints?.Invoke(1);
                _activeController = ActiveController.Player;
                break;
        }
    }
}