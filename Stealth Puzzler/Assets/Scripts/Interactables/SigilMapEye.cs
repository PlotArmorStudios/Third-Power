using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering.Universal;

public class SigilMapEye : MonoBehaviour
{
    [SerializeField] private bool _isActive = true;
    [SerializeField] private UnityEvent _eyeTriggerEvent;
    [SerializeField] private bool _oscillate;

    [Header("Emission Control")] [SerializeField]
    private Material _emissionMaterial;

    [SerializeField] private float _litOnTime = 1f;
    [SerializeField] private float _lightingTime = 1f;
    [SerializeField] private float _emissionMax = 4f;
    [SerializeField] private float _emissionMin = 1.4f;
    [SerializeField] private float _frequency = 1f;

    [SerializeField] private bool _useTimer = false;

    public bool IsLit { get; private set; }
    private Color _emissionColor;

    private void OnEnable()
    {
        GetComponent<DecalProjector>().material = Instantiate<Material>(GetComponent<DecalProjector>().material);
        _emissionMaterial = GetComponent<DecalProjector>().material;
    }

    private void Start()
    {
        _emissionColor = _emissionMaterial.GetColor("_Emissive_color");
        _emissionMaterial.SetColor("_Emissive_color", _emissionColor * _emissionMin);
    }

    public IEnumerator OscillateEmissionTimer()
    {
        Deactivate();
        IsLit = true;
        float timeElapsed = 0f;
        float currentEmission = _emissionMin;

        while (timeElapsed < _litOnTime)
        {
            currentEmission = Mathf.Lerp(currentEmission, _emissionMax, timeElapsed / _lightingTime);
            timeElapsed += Time.deltaTime;
            _emissionMaterial.SetColor("_Emissive_color", _emissionColor * currentEmission);
#if DebugTimeTransition
            Debug.Log("Lerp progress: " + timeElasped / _frequency);
            Debug.Log("Time Elapsed: " + timeElasped);
            Debug.Log("Time: " + Time.deltaTime);
            Debug.Log("Current emission" + currentEmission);
#endif
            yield return null;
        }

        timeElapsed = 0;

        while (timeElapsed < _lightingTime)
        {
            currentEmission = Mathf.Lerp(currentEmission, _emissionMin, timeElapsed / _lightingTime);
            timeElapsed += Time.deltaTime;
            _emissionMaterial.SetColor("_Emissive_color", _emissionColor * currentEmission);
#if DebugTimeTransition
            Debug.Log(timeElasped / _frequency);
            Debug.Log(currentEmission);
#endif
            yield return null;
        }

        IsLit = false;
        Activate();
    }

    public IEnumerator OscillateEmission()
    {
        float timeElasped = 0f;
        float currentEmission = _emissionMin;

        while (timeElasped < _frequency)
        {
            currentEmission = Mathf.Lerp(currentEmission, _emissionMax, timeElasped / _frequency);
            timeElasped += Time.deltaTime;
            _emissionMaterial.SetColor("_Emissive_color", _emissionColor * currentEmission);
#if DebugTimeTransition
            Debug.Log("Lerp progress: " + timeElasped / _frequency);
            Debug.Log("Time Elapsed: " + timeElasped);
            Debug.Log("Time: " + Time.deltaTime);
            Debug.Log("Current emission" + currentEmission);
#endif
            yield return null;
        }

        if (!_oscillate) yield break;
        timeElasped = 0;

        while (timeElasped < _frequency)
        {
            currentEmission = Mathf.Lerp(currentEmission, _emissionMin, timeElasped / _frequency);
            timeElasped += Time.deltaTime;
            _emissionMaterial.SetColor("_Emissive_color", _emissionColor * currentEmission);
#if DebugTimeTransition
            Debug.Log(timeElasped / _frequency);
            Debug.Log(currentEmission);
#endif
            yield return null;
        }
    }

    [ContextMenu("Oscillate Emission")]
    private void TestOscRoutine()
    {
        StartCoroutine(OscillateEmission());
    }

    public void Activate()
    {
        _isActive = true;
    }

    public void Deactivate()
    {
        _isActive = false;
    }

    public void IlluminateMap()
    {
        _eyeTriggerEvent?.Invoke();

        if (_useTimer)
        {
            StartCoroutine(OscillateEmissionTimer());
        }
        else
        {
            StartCoroutine(OscillateEmission());
        }

        PlayEyeGlowSound();
    }

    private void PlayEyeGlowSound()
    {
        //Implement blue eye glow sound
        AkSoundEngine.PostEvent("Play_Eye_Impact", gameObject);
    }
}