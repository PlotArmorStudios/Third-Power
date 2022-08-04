using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFader : MonoBehaviour
{
    [SerializeField] private float _transitionSpeed = 2f;
    [SerializeField] private float _fadePauseTime = 1f;
    private Image _fadeScreen;

    private void Start()
    {
        _fadeScreen = GetComponent<Image>();
    }

    [ContextMenu("Fade Black")]
    public void Fade()
    {
        StartCoroutine(PlayTransition());
    }
    
    private IEnumerator PlayTransition()
    {
        float alpha = 0;

        while (alpha < 1)
        {
            alpha += _transitionSpeed * Time.unscaledDeltaTime;
            var newColor = new Color(0, 0, 0, alpha);
            _fadeScreen.color = newColor;
            yield return null;
        }

        yield return new WaitForSeconds(_fadePauseTime);
        
        while (alpha > 0)
        {
            alpha -= _transitionSpeed * Time.unscaledDeltaTime;
            var newColor = new Color(0, 0, 0, alpha);
            _fadeScreen.color = newColor;
            yield return null;
        }
    }
}
