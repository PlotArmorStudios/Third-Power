using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class TriggerVignette : MonoBehaviour
{
    [SerializeField] private Volume _globalVolume;
    private void OnEnable() => PlayerHealth.OnPlayerDie += TriggerOnVignette;
    private void OnDisable() => PlayerHealth.OnPlayerDie -= TriggerOnVignette;

    private void TriggerOnVignette()
    {
        _globalVolume.weight = 1f;
    }
}