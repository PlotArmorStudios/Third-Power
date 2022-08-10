using System;
using UnityEngine;

public class WwiseVoiceOver : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event VO1;
    [SerializeField] private AK.Wwise.Event VO_Pause;
    [SerializeField] private AK.Wwise.Event VO_Resume;

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
}
