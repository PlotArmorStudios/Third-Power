using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlueEye : MonoBehaviour
{
    [SerializeField] private UnityEvent _collisionEvent;
    [SerializeField] private Material _emissionMaterial;
    [SerializeField] private float _period = 1f; // Time for 1 oscillation (1 sec)
    [SerializeField] private float _emissionMax = 4f;
    [SerializeField] private float _emissionMin = 1.4f;
    [SerializeField] private float _frequency = 1f;

    public IEnumerator OscillateEmission()
    {
        float timeElasped = 0f;
        float cycles = 0f;
        float rawSineWave = 0f;
        float currentEmission = _emissionMin;
        const float tau = Mathf.PI * 2;
        cycles = Time.time / _period; 
        //rawSineWave = Mathf.Sin(Time.time * _frequency * _emissionMin) * _emissionMax;

        while (timeElasped < _frequency)
        {
            currentEmission = Mathf.Lerp(currentEmission, _emissionMax, timeElasped * _frequency);
            timeElasped += Time.deltaTime;
            yield return currentEmission == _emissionMax;
            currentEmission = Mathf.Lerp(currentEmission, _emissionMin, timeElasped * _frequency);
            yield return null;
            Debug.Log(currentEmission);
        }
    }

    [ContextMenu("Oscillate Emission")]
    private void TestOscRoutine()
    {
        StartCoroutine(OscillateEmission());
    }

    private void OnCollisionEnter(Collision other)
    {
        var projectile = other.gameObject.GetComponent<Projectile>();
        if (!projectile) return;
        _collisionEvent?.Invoke();
        StartCoroutine(OscillateEmission());
    }
}