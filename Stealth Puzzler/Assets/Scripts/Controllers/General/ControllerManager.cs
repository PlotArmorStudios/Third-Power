using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class ControllerManager : MonoBehaviour
{
    public static event Action<int> OnSwitchFocalPoints;
    [SerializeField] private InputActionReference _switch;
    [SerializeField] private InputActionReference _airborneSwitch;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private CubeController _cubeController;
    [SerializeField] private ActiveController _startingController = ActiveController.Player;
    [SerializeField] private GameObject _poofObject;

    [SerializeField] [Tooltip("For adjusting height after switch")]
    private LayerMask _groundLayerMask;

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
        _airborneSwitch.action.Enable();
    }

    private void OnDisable()
    {
        _switch.action.Disable();
        _airborneSwitch.action.Disable();
    }

    private void Start()
    {
        PlayerIsActive = true;
        _poofEffect = _poofObject.GetComponentInChildren<ParticleSystem>();
        //SetStartingController();
    }

    public void InitializeControllers(Camera main)
    {
        var controllers = GetComponentsInChildren<Controller>();

        foreach (var controller in controllers)
        {
            Debug.Log("Initialized cam: " + main);
            controller.InitializeCam(main);
        }
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
            SwitchControllers();
        if (_airborneSwitch.action.triggered)
            AirborneSwitch();
    }

    //true if successful, false if unable to switch
    public bool SwitchControllers()
    {
        if (SwitchingBlocked)
        {
            return false;
        }

        var currentControllerPosition = Vector3.zero;

        switch (_activeController)
        {
            case ActiveController.Player:
                currentControllerPosition = ForceCube();
                break;
            case ActiveController.Cube:
                if (_cubeController.GetIsMoving()) return false;
                currentControllerPosition = ForceHuman();
                break;
        }

        AkSoundEngine.PostEvent("Play_Character_Cube_Transform", gameObject);
        _poofEffect.transform.position = currentControllerPosition;
        _poofEffect.gameObject.SetActive(true);
        _poofEffect.Play();
        return true;
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
        RaycastHit hit;
        if (Physics.Raycast(_cubeController.transform.position, Vector3.down, out hit, 2f, _groundLayerMask))
            _cubeController.transform.position = hit.point;
        _activeController = ActiveController.Cube;
        return currentControllerPosition;
    }

    //Y position after switch must be adjusted to avoid cube sticking in floor
    private void AdjustPosition(ref Vector3 position, GameObject activeController)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 2f, _groundLayerMask))
            position = hit.point;
    }

    public void AirborneSwitch()
    {
        if (!_playerController.GroundCheck.IsGrounded())
        {
            SwitchControllers();
            _cubeController.Drop.AerialDrop();
        }
    }

    /// <summary>
    /// Used to deparent both controllers when exiting moving platforms regardless
    /// of active controller.
    /// </summary>
    public void DeparentControllers()
    {
        _playerController.transform.SetParent(null);
        _cubeController.transform.SetParent(null);
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