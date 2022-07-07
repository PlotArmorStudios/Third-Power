using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlueEye : MonoBehaviour
{
    [SerializeField] private bool _isActive = true;
    [SerializeField] private UnityEvent _collisionEvent;
    [SerializeField] private Material _emissionMaterial;
    [SerializeField] private float _stayOnTime = 1f;
    [SerializeField] private float _emissionMax = 4f;
    [SerializeField] private float _emissionMin = 1.4f;
    [SerializeField] private float _lightingTime = 1f;
    public bool IsLit { get; private set; }
    public static event Action OnEyeHit;
    private Color _emissionColor;
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

    public IEnumerator OscillateEmission()
    {
        Deactivate();
        IsLit = true;
        OnEyeHit?.Invoke();
        float timeElapsed = 0f;
        float currentEmission = _emissionMin;

        while (timeElapsed < _stayOnTime)
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
    
    private void OnCollisionEnter(Collision other)
    {
        var projectile = other.gameObject.GetComponent<Projectile>();
        if (!projectile) return;
        if (!_isActive) return;
        
        _collisionEvent?.Invoke();
        StartCoroutine(OscillateEmission());
        PlayPuzzleSolvedSound();
        PlayEyeGlowSound();
    }

    private void PlayEyeGlowSound()
    {
        //Implement blue eye glow sound
        AkSoundEngine.PostEvent("Play_Eye_Impact", gameObject);
    }

    private void PlayPuzzleSolvedSound()
    {
        //Implement puzzle solved sound
        //Let this be a sound that fires off immediately.
        //The programmers may play around with the timing of the sound later
    }
}
