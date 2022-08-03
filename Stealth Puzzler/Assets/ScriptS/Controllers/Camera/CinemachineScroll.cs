using Cinemachine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Threading.Tasks;

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

    

    private CinemachineFreeLook _vCam;
    private GameInput _input;
    private float _scroll;

    private float _topScroll;
    private float _midScroll;
    private float _bottomScroll;

    private float _topHeight;
    private float _midHeight;

    [SerializeField] private InputActionReference _scrollInput;
    [SerializeField] private float _topHeightZoomSize = 0.75f;
    [SerializeField] private float _midHeightZoomSize = 0.5f;

    [SerializeField] private float _zoomSpeed = 0.1f;
    [SerializeField] private int _timeUntilRepeat = 100;

    private void Awake()
    {

    }

    private void OnEnable()
    {
        _scrollInput.action.performed += ReadScrollValue;
    }

    private void OnDisable()
    {
        _scrollInput.action.performed -= ReadScrollValue;
    }

    private async void ReadScrollValue(InputAction.CallbackContext context)
    {
        _scroll = context.ReadValue<float>();
        while (_scroll != 0)
        {
            Zoom();
            await Task.Delay(_timeUntilRepeat);
        }
    }
    private void Start()
    {
        _vCam = GetComponent<CinemachineFreeLook>();
        _topHeight = _vCam.m_Orbits[0].m_Height;
        _midHeight = _vCam.m_Orbits[1].m_Height;

        _topScroll = _vCam.m_Orbits[0].m_Radius;
        _midScroll = _vCam.m_Orbits[1].m_Radius;
        _bottomScroll = _vCam.m_Orbits[2].m_Radius;
    }

    private void Zoom()
    {
        var _tempMidRadiusVar = _midScroll;
        var _tempBottomRadiusVar = _bottomScroll;

        var _tempTopHeightVar = _topHeight;
        var _tempMidHeightVar = _midHeight;
        if (_scroll < 0)
        {
            if (_midScroll + _scrollSensitivity <= _midRigMaxScroll)
            {
                _midScroll += _scrollSensitivity;
                _bottomScroll += _scrollSensitivity;
                LeanTween.value(_vCam.gameObject, SetMidRadiusCallback, _tempMidRadiusVar, _midScroll, _zoomSpeed);
                LeanTween.value(_vCam.gameObject, SetBotRadiusCallback, _tempBottomRadiusVar, _bottomScroll, _zoomSpeed);

                _topHeight = _topHeight + _topHeightZoomSize;
                _midHeight = _midHeight + _midHeightZoomSize;
                LeanTween.value(_vCam.gameObject, SetTopHeightCallback, _tempTopHeightVar, _topHeight, _zoomSpeed);
                LeanTween.value(_vCam.gameObject, SetMidHeightCallback, _tempMidHeightVar, _midHeight, _zoomSpeed);
            } 
        }

        if (_scroll > 0)
        {
            if (_midScroll - _scrollSensitivity >= _midRigMinScroll)
            {
                _midScroll -= _scrollSensitivity;
                _bottomScroll -= _scrollSensitivity;
                LeanTween.value(_vCam.gameObject, SetMidRadiusCallback, _tempMidRadiusVar, _midScroll, _zoomSpeed);
                LeanTween.value(_vCam.gameObject, SetBotRadiusCallback, _tempBottomRadiusVar, _bottomScroll, _zoomSpeed);

                _topHeight = _topHeight - _topHeightZoomSize;
                _midHeight = _midHeight - _midHeightZoomSize;
                LeanTween.value(_vCam.gameObject, SetTopHeightCallback, _tempTopHeightVar, _topHeight, _zoomSpeed);
                LeanTween.value(_vCam.gameObject, SetMidHeightCallback, _tempMidHeightVar, _midHeight, _zoomSpeed);
            }
        }
    }
    private void SetMidRadiusCallback(float c)
    {
        _vCam.m_Orbits[1].m_Radius = c;
    }
    private void SetBotRadiusCallback(float c)
    {
        _vCam.m_Orbits[2].m_Radius = c;
    }

    private void SetTopHeightCallback(float c)
    {
        _vCam.m_Orbits[0].m_Height = c;
    }
    private void SetMidHeightCallback(float c)
    {
        _vCam.m_Orbits[1].m_Height = c;
    }

}