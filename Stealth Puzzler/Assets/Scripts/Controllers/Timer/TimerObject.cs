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
        yield return new WaitForSeconds(_timerDuration);
        _timerEnd?.Invoke();
    }
}
