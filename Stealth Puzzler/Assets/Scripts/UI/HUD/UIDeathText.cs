using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDeathText : MonoBehaviour
{
    [SerializeField] private RectTransform _UIDeathScreenCanvas;
    private float _tweenTime = 0.5f;
    private float _delayTime = 1f;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDie += FlashDeathScreen;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDie -= FlashDeathScreen;
    }
    private void FlashDeathScreen()
    {
        LeanTween.alpha(_UIDeathScreenCanvas, 1f, _tweenTime).setFrom(0).setDelay(_delayTime);
    }
}
