using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class WwiseVoiceOver : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event VO1;
    [SerializeField] private AK.Wwise.Event VO_Pause;
    [SerializeField] private AK.Wwise.Event VO_Resume;

    [SerializeField] private UnityEvent _showSubtitleEvent;
    [SerializeField] private UnityEvent _subtitleClearEvent;
    [SerializeField] private float _subtitleShowTime = 2f;
    [SerializeField] private float _subtitleClearTime = 2f;

    private bool _firstTime = true;

    private void OnEnable()
    {
        PauseMenu.OnPause += Pause;
        PauseMenu.OnResume += Resume;
    }

    private void OnDisable()
    {
        PauseMenu.OnPause -= Pause;
        PauseMenu.OnResume -= Resume;
    }

    void OnTriggerEnter()
    {
        if (_firstTime)
        {
            VO1.Post(gameObject);
            TriggerSubtitleEvent();
            _firstTime = false;
        }
    }

    private void Pause()
    {
        VO_Pause.Post(gameObject);
    }

    private void Resume()
    {
        VO_Resume.Post(gameObject);
    }

    public void TriggerSubtitleEvent()

    {
        StartCoroutine(ShowSubtitle());
    }

    private IEnumerator ShowSubtitle()
    {
        yield return new WaitForSeconds(_subtitleShowTime);
        _showSubtitleEvent?.Invoke();
        StartCoroutine(ClearSubtitle());
    }

    private IEnumerator ClearSubtitle()
    {
        yield return new WaitForSeconds(_subtitleClearTime);
        _subtitleClearEvent?.Invoke();
    }
}