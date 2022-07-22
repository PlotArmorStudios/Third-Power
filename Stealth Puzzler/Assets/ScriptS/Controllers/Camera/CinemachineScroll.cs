using Cinemachine;
using UnityEngine;

public class CinemachineScroll : MonoBehaviour
{
    [SerializeField] private float _scrollSensitivity = .1f;
    
    private CinemachineFreeLook _vCam;
    private GameInput _input;
    private float _scroll;

    private void Awake()
    {
        _input = new GameInput();
        _input.Player.Scroll.performed += x => _scroll = x.ReadValue<float>();
    }

    private void OnEnable() => _input.Enable();

    private void Start()
    {
        _vCam = GetComponent<CinemachineFreeLook>();
    }

    private void Update()
    {
        ReadScrollInput();
    }

    private void ReadScrollInput()
    {
        if (_scroll < 0)
        {
            _vCam.m_Orbits[0].m_Radius -= _scrollSensitivity * Mathf.Abs(_scroll);
            _vCam.m_Orbits[1].m_Radius -= _scrollSensitivity * Mathf.Abs(_scroll);
            _vCam.m_Orbits[2].m_Radius -= _scrollSensitivity * Mathf.Abs(_scroll);
        }

        if (_scroll > 0)
        {
            _vCam.m_Orbits[0].m_Radius += _scrollSensitivity * Mathf.Abs(_scroll);
            _vCam.m_Orbits[1].m_Radius += _scrollSensitivity * Mathf.Abs(_scroll);
            _vCam.m_Orbits[2].m_Radius += _scrollSensitivity * Mathf.Abs(_scroll);
        }
    }
}