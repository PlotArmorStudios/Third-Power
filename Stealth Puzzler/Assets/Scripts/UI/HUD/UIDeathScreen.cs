using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIDeathScreen : MonoBehaviour
{
    [SerializeField] private RectTransform _UIDeathScreenCanvas;
    private float _tweenTime = 0.5f;

    private void OnEnable()
    {
        PlayerHealth.OnPlayerDeath += FlashDeathScreen;
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDeath -= FlashDeathScreen;
    }
    private void FlashDeathScreen(float obj)
    {
        LeanTween.alpha(_UIDeathScreenCanvas, 1f, _tweenTime).setFrom(0);
    }
}
