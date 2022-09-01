using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UIOscillator : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private float _oscillateTime = 1.5f;
    [SerializeField] private float _magnitude = .5f;
    [SerializeField] private float _frequency = .5f;
    private float _currentTime;

    [ContextMenu("Trigger UI")]
    public void TriggerUI()
    {
        StopCoroutine(SetOscillate());
        StartCoroutine(SetOscillate());
    }

    private IEnumerator SetOscillate()
    {
        _currentTime = 0;
        _text.alpha = 0;
        
        while (_currentTime < _oscillateTime)
        {
            _text.alpha = Mathf.Sin(_frequency * Time.time) * _magnitude;
            _currentTime += Time.deltaTime;
            yield return null;
        }

        _text.alpha = 0;
    }
}