using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class TimerObject : MonoBehaviour
{
    [SerializeField] private UnityEvent _timerStart;
    [SerializeField] private UnityEvent _timerEnd;
    [SerializeField] private float _timerDuration;
    [SerializeField] private bool _IsOpeningDoor;
    public static Action<float> OnSendDoorTime;
    
    public void TriggerTimer()
    {
        //insert code to stop all timer sounds
        StopAllCoroutines();
        StartCoroutine(StartTimer());
        if (_IsOpeningDoor)
        {
            OnSendDoorTime?.Invoke(_timerDuration);
        }
    }
    
    IEnumerator StartTimer()
    {
        _timerStart?.Invoke();
        AkSoundEngine.PostEvent("Play_puzzle_time_running_out", gameObject);
        yield return new WaitForSeconds(_timerDuration);
        _timerEnd?.Invoke();
        AkSoundEngine.PostEvent("stop_puzzle_time_running_out", gameObject);
    }
}
