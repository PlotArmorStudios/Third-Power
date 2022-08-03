using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDoorTimer : MonoBehaviour
{
    [SerializeField] private GameObject _timerPanel;
    [SerializeField] private List<Image> _timerDots;
    [SerializeField] private float _initialDotScale = 0.5f;
    private float _timer;
    private float _timeForDot;
    

    private void OnEnable()
    {
        TimerObject.OnSendDoorTime += ShowDoorTimer;
    }

    private void OnDisable()
    {
        TimerObject.OnSendDoorTime -= ShowDoorTimer;
    }

    private IEnumerator TimerDot()
    {
        foreach (var _dot in _timerDots)
        {
            LeanTween.cancel(_dot.gameObject);
            LeanTween.scale(_dot.gameObject, Vector3.zero, _timeForDot);
            yield return new WaitForSeconds(_timeForDot);
        }
        _timerPanel.SetActive(false);
    }

    private void ShowDoorTimer(float _doorOpenTime)
    {
        ResetDots();
        _timer = _doorOpenTime;
        _timeForDot = _timer / _timerDots.Count;
        _timerPanel.SetActive(true);
        StartCoroutine(TimerDot());
    }

    private void ResetDots()
    {
        StopAllCoroutines();
        foreach (var _dot in _timerDots)
        {
            LeanTween.cancel(_dot.gameObject);
            _dot.gameObject.transform.localScale = Vector3.one * _initialDotScale;
        }
    }
}
