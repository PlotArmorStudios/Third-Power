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
    [SerializeField] private ActiveController _startingController = ActiveController.Player;
    [SerializeField] private GameObject _poofObject;

    public List<Transform> FocalPoints;
    public static ControllerManager Instance;
    private ParticleSystem _poofEffect;

    private ActiveController _activeController = ActiveController.Player;
    public bool PlayerIsActive { get; set; }
    public bool SwitchingBlocked = false;

    private void Awake()
    {
        Instance = this;
    }

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
        PlayerIsActive = true;
        _poofEffect = _poofObject.GetComponentInChildren<ParticleSystem>();
        //SetStartingController();
    }

    private void SetStartingController()
    {
        if (_startingController == ActiveController.Player)
        {
            _activeController = ActiveController.Cube;
            SwitchControllers();
        }

        if (_startingController == ActiveController.Cube)
        {
            _activeController = ActiveController.Player;
            SwitchControllers();
        }
    }

    private void Update()
    {
        if (_switch.action.triggered)
        {
            SwitchControllers();
            AkSoundEngine.PostEvent("Play_Character_Cube_Transform", gameObject);
        }
    }

    public void SwitchControllers()
    {
        if (SwitchingBlocked)
        {
            return;
        }
        var currentControllerPosition = Vector3.zero;

        switch (_activeController)
        {
            case ActiveController.Player:
                currentControllerPosition = ForceCube();
                break;
            case ActiveController.Cube:
                currentControllerPosition = ForceHuman();
                break;
        }

        _poofEffect.transform.position = currentControllerPosition;
        _poofEffect.gameObject.SetActive(true);
        _poofEffect.Play();
    }

    public Vector3 ForceHuman()
    {
        Vector3 currentControllerPosition;
        var distance = Vector3.Distance(_playerController.transform.position,
_playerController.CubeCalibratorTransform.position);

        _playerController.transform.position = _cubeController.transform.position +
                                               (Vector3.up * distance);

        _playerController.gameObject.SetActive(true);
        _cubeController.gameObject.SetActive(false);
        currentControllerPosition = _playerController.Rigidbody.transform.position;
        OnSwitchFocalPoints?.Invoke(1);
        _activeController = ActiveController.Player;
        return currentControllerPosition;
    }

    public Vector3 ForceCube()
    {
        Vector3 currentControllerPosition;
        _cubeController.transform.position = _playerController.CubeCalibratorTransform.position;
        _playerController.gameObject.SetActive(false);
        _cubeController.gameObject.SetActive(true);
        OnSwitchFocalPoints?.Invoke(2);
        currentControllerPosition = _cubeController.Rigidbody.transform.position;
        _activeController = ActiveController.Cube;
        return currentControllerPosition;
    }

    public void DeactivateControllers()
    {
        _playerController.GetComponent<ToggleComponents>().ToggleOffComponents();
        _cubeController.GetComponent<ToggleComponents>().ToggleOffComponents();
    }

    public void ActivateControllers()
    {
        _playerController.GetComponent<ToggleComponents>().ToggleOnComponents();
        _cubeController.GetComponent<ToggleComponents>().ToggleOnComponents();
    }

    /// <summary>
    /// Used to toggle an enemy's ability to detect player (on death).
    /// </summary>
    public void ActivatePlayer()
    {
        PlayerIsActive = true;
    }
    public void DeactivatePlayer()
    {
        PlayerIsActive = false;
    }
}