using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlueEye : MonoBehaviour
{
    [SerializeField] private bool _isActive = true;
    [SerializeField] private UnityEvent _collisionEvent;
    
    [Header("Emission Control")]
    [SerializeField] private Material _emissionMaterial;
    [SerializeField] private float _litOnTime = 1f;
    [SerializeField] private float _emissionMax = 4f;
    [SerializeField] private float _emissionMin = 1.4f;
    [SerializeField] private float _frequency = 1f;
    [SerializeField] private float _lightingTime = 1f;
    
    [Header("Blue Eye Functionality")]
    [SerializeField] private bool _deactivatesOnCollision = true;
    [SerializeField] private bool _useTimer = false;

    [SerializeField] private int _numberOfTriggers = 1;
    public bool IsLit { get; private set; }
    public static event Action OnEyeHit;
    private Color _emissionColor;

    [SerializeField] private bool _playPuzzleSolveSound;
    [SerializeField] private float _puzzleSoundWaitTime;

    private void OnEnable()
    {
        GetComponent<MeshRenderer>().material = Instantiate<Material>(GetComponent<MeshRenderer>().material);
        _emissionMaterial = GetComponent<MeshRenderer>().material;
    }

    private void Start()
    {
        _emissionColor = _emissionMaterial.color;
        _emissionMaterial.SetColor("_EmissionColor", _emissionColor * _emissionMin);
    }

    public IEnumerator OscillateEmissionTimer()
    {
        Deactivate();
        IsLit = true;
        OnEyeHit?.Invoke();
        float timeElapsed = 0f;
        float currentEmission = _emissionMin;

        while (timeElapsed < _litOnTime)
        {
            currentEmission = Mathf.Lerp(currentEmission, _emissionMax, timeElapsed / _lightingTime);
            timeElapsed += Time.deltaTime;
            _emissionMaterial.SetColor("_EmissionColor", _emissionColor * currentEmission);
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
            _emissionMaterial.SetColor("_EmissionColor", _emissionColor * currentEmission);
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
        if (_deactivatesOnCollision)
            Deactivate();

        float timeElasped = 0f;
        float currentEmission = _emissionMin;

        while (timeElasped < _frequency)
        {
            currentEmission = Mathf.Lerp(currentEmission, _emissionMax, timeElasped / _frequency);
            timeElasped += Time.deltaTime;
            _emissionMaterial.SetColor("_EmissionColor", _emissionColor * currentEmission);
#if DebugTimeTransition
            Debug.Log("Lerp progress: " + timeElasped / _frequency);
            Debug.Log("Time Elapsed: " + timeElasped);
            Debug.Log("Time: " + Time.deltaTime);
            Debug.Log("Current emission" + currentEmission);
#endif
            yield return null;
        }

        timeElasped = 0;

        while (timeElasped < _frequency)
        {
            currentEmission = Mathf.Lerp(currentEmission, _emissionMin, timeElasped / _frequency);
            timeElasped += Time.deltaTime;
            _emissionMaterial.SetColor("_EmissionColor", _emissionColor * currentEmission);
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
        StartCoroutine(OscillateEmissionTimer());
    }

    public void Activate()
    {
        _isActive = true;
    }

    public void Deactivate()
    {
        _isActive = false;
    }

    private void OnCollisionEnter(Collision other)
    {
        var projectile = other.gameObject.GetComponent<Projectile>();
        if (!projectile) return;
        if (!_isActive) return;

#if DebugLog
        Debug.Log("Collided with: " + other.gameObject.name);
#endif
        _collisionEvent?.Invoke();
        if (_useTimer)
        {
            StartCoroutine(OscillateEmissionTimer());
        }
        else
        {
            StopCoroutine(OscillateEmission());
            StartCoroutine(OscillateEmission());
        }

        PlayEyeGlowSound();

        if (_playPuzzleSolveSound)
            StartCoroutine(PlayPuzzleSolvedSound());
    }

    private void PlayEyeGlowSound()
    {
        //Implement blue eye glow sound
        AkSoundEngine.PostEvent("Play_Eye_Impact", gameObject);
    }

    private IEnumerator PlayPuzzleSolvedSound()
    {
        //Implement puzzle solved sound
        //Let this be a sound that fires off immediately.
        //The programmers may play around with the timing of the sound later
        yield return new WaitForSeconds(_puzzleSoundWaitTime);
        AkSoundEngine.PostEvent("Play_puzzle_solved", gameObject);
    }
}