using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerObject : MonoBehaviour
{
    [SerializeField] private UnityEvent _timerStart;
    [SerializeField] private UnityEvent _timerEnd;
    
    [SerializeField] private float _timerDuration;
    
    public void TriggerTimer()
    {
        StartCoroutine(StartTimer());
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
