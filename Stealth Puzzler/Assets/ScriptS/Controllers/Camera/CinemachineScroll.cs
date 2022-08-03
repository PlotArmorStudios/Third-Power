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

    [SerializeField] private float _zoomSpeed = 0.25f;

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

    [SerializeField] private float _zoomRepeatRate = 0.25f;
    [SerializeField] private int _timeUntilRepeat = 250;

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

    private void LateUpdate()
    {
        
        //_timeUntilRepeat -= Time.deltaTime;
        //if (_timeUntilRepeat < 0)
        //{
        //    Zoom();
        //    _timeUntilRepeat = _zoomRepeatRate;
        //}
    }
    private async void ReadScrollValue(InputAction.CallbackContext context)
    {
        _scroll = context.ReadValue<float>();
        while (_scroll != 0)
        {
            Zoom();
            await Task.Delay(_timeUntilRepeat);
        }
        
        //float z = context.ReadValue<float>();
        //if (z > 0)
        //{
        //    _scroll = z;
        //    Zoom();
        //}

        //else if (z < 0)
        //{
        //    _scroll = z;
        //    Zoom();
        //}
    }
    private void Start()
    {
        _vCam = GetComponent<CinemachineFreeLook>();
        _topHeight = _vCam.m_Orbits[0].m_Height;
        _midHeight = _vCam.m_Orbits[1].m_Height;
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