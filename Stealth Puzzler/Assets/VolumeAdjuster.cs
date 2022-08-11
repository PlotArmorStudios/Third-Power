using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class VolumeAdjuster : MonoBehaviour
{
    [SerializeField] private float _fadeSpeed = 1;
    private Volume _volume;

    private void Start()
    {
        _volume = GetComponent<Volume>();
        _volume.weight = 0;
    }

    [ContextMenu("Set Volume")]
    public void SetVolume()
    {
        StartCoroutine(FadeVolume());
    }

    private IEnumerator FadeVolume()
    {
        var volumeAlpha = 0f;

        while (volumeAlpha < 1)
        {
            volumeAlpha += Time.deltaTime * _fadeSpeed;
            _volume.weight = volumeAlpha;
            yield return null;
        }
    }
}