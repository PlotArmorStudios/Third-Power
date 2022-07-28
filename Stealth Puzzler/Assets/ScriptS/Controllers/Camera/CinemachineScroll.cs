using Cinemachine;
using UnityEngine;

public class CinemachineScroll : MonoBehaviour
{
    [SerializeField] private float _scrollSensitivity = .1f;

    [Header("Top Rig Scroll")] [SerializeField]
    private float _topRigMaxScroll = 20f;

    [SerializeField] private float _topRigMinScroll = 8f;

    [Header("Middle Rig Scroll")] [SerializeField]
    private float _midRigMaxScroll = 25f;

    [SerializeField] private float _midRigMinScroll = 10f;

    [Header("Bottom Rig Scroll")] [SerializeField]
    private float _bottomRigMaxScroll = 20f;

    [SerializeField] private float _bottomRigMinScroll = 8f;

    private CinemachineFreeLook _vCam;
    private GameInput _input;
    private float _scroll;

    private float _topScroll;
    private float _midScroll;
    private float _bottomScroll;

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

    private void LateUpdate()
    {
        ReadScrollInput();
        _topScroll = Mathf.Clamp(_topScroll, _topRigMinScroll, _topRigMaxScroll);
        _midScroll = Mathf.Clamp(_midScroll, _midRigMinScroll, _midRigMaxScroll);
        _bottomScroll = Mathf.Clamp(_bottomScroll, _bottomRigMinScroll, _bottomRigMaxScroll);
    }

    private void ReadScrollInput()
    {
        if (_scroll < 0)
        {
            if (_topScroll != _topRigMaxScroll)
                _topScroll += _scrollSensitivity * Mathf.Abs(_scroll);
            if (_midScroll != _midRigMaxScroll)
                _midScroll += _scrollSensitivity * Mathf.Abs(_scroll);
            if (_bottomScroll != _bottomRigMaxScroll)
                _bottomScroll += _scrollSensitivity * Mathf.Abs(_scroll);
        }

        if (_scroll > 0)
        {
            if (_topScroll != _topRigMinScroll)
                _topScroll -= _scrollSensitivity * Mathf.Abs(_scroll);
            if (_midScroll != _midRigMinScroll)
                _midScroll -= _scrollSensitivity * Mathf.Abs(_scroll);
            if (_bottomScroll != _bottomRigMinScroll)
                _bottomScroll -= _scrollSensitivity * Mathf.Abs(_scroll);
        }

        _vCam.m_Orbits[0].m_Radius = _topScroll;
        _vCam.m_Orbits[1].m_Radius = _midScroll;
        _vCam.m_Orbits[2].m_Radius = _bottomScroll;
    }
}