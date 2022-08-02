using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CinemachineScroll : MonoBehaviour
{
    [SerializeField] private float _scrollSensitivity = .1f;

    [Header("Top Rig Scroll")] [SerializeField]
    private float _topRigMaxScroll = 10f;

    [SerializeField] private float _topRigMinScroll = 4f;

    [Header("Middle Rig Scroll")] [SerializeField]
    private float _midRigMaxScroll = 10f;

    [SerializeField] private float _midRigMinScroll = 4f;

    [Header("Bottom Rig Scroll")] [SerializeField]
    private float _bottomRigMaxScroll = 10f;

    [SerializeField] private float _bottomRigMinScroll = 4f;

    [SerializeField] private float _zoomSpeed = 0.25f;

    private CinemachineFreeLook _vCam;
    private GameInput _input;
    private float _scroll;

    private float _topScroll;
    private float _midScroll;
    private float _bottomScroll;

    [SerializeField] private InputActionReference _scrollInput;

    private void Awake()
    {
        _topScroll = _topRigMinScroll;
        _midScroll = _midRigMinScroll;
        _bottomScroll = _bottomRigMinScroll;
    }

    private void OnEnable()
    {
        _scrollInput.action.performed += ReadScrollValue;
    }

    private void OnDisable()
    {
        _scrollInput.action.performed -= ReadScrollValue;
    }
    private void ReadScrollValue(InputAction.CallbackContext context)
    {
        float z = context.ReadValue<float>();
        if (z > 0)
        {
            _scroll = z;
            ReadScrollInput();
        }
            
        else if (z < 0)
        {
            _scroll = z;
            ReadScrollInput();
        }
    }
    private void Start()
    {
        _vCam = GetComponent<CinemachineFreeLook>();
    }

    private void ReadScrollInput()
    {
        var _tempTopVar = _topScroll;
        var _tempMidVar = _midScroll;
        var _tempBottomVar = _bottomScroll;
        if (_scroll < 0)
        {
            if (_topScroll + _scrollSensitivity <= _topRigMaxScroll)
            {
                _topScroll += _scrollSensitivity;
                _midScroll += _scrollSensitivity;
                _bottomScroll += _scrollSensitivity;
                LeanTween.value(_vCam.gameObject, SetTopCallback, _tempTopVar, _topScroll, _zoomSpeed);
                LeanTween.value(_vCam.gameObject, SetMidCallback, _tempMidVar, _midScroll, _zoomSpeed);
                LeanTween.value(_vCam.gameObject, SetBotCallback, _tempBottomVar, _bottomScroll, _zoomSpeed);
            } 
        }

        if (_scroll > 0)
        {
            if (_topScroll - _scrollSensitivity >= _topRigMinScroll)
            {
                _topScroll -= _scrollSensitivity;
                _midScroll -= _scrollSensitivity;
                _bottomScroll -= _scrollSensitivity;
                LeanTween.value(_vCam.gameObject, SetTopCallback, _tempTopVar, _topScroll, _zoomSpeed);
                LeanTween.value(_vCam.gameObject, SetMidCallback, _tempMidVar, _midScroll, _zoomSpeed);
                LeanTween.value(_vCam.gameObject, SetBotCallback, _tempBottomVar, _bottomScroll, _zoomSpeed);
            }
        }
    }

    private void SetTopCallback(float c)
    {
        _vCam.m_Orbits[0].m_Radius = c;
    }
    private void SetMidCallback(float c)
    {
        _vCam.m_Orbits[1].m_Radius = c;
    }
    private void SetBotCallback(float c)
    {
        _vCam.m_Orbits[2].m_Radius = c;
    }
}