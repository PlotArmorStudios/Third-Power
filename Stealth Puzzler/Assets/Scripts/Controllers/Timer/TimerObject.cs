using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TimerObject : MonoBehaviour
{
    [SerializeField] private UnityEvent unityEvent1;
    [SerializeField] private UnityEvent unityEvent2;
    [SerializeField] private DoorManager _door;
    [SerializeField] private float _timerDuration;
    // Start is called before the first frame update
    public void TriggerTimer()
    {
        StartCoroutine(StartTimer());
    }
    IEnumerator StartTimer()
    {
        unityEvent1?.Invoke();
        yield return new WaitForSeconds(_timerDuration);
        unityEvent2?.Invoke();
    }

    public void OpenDoor()
    {
        _door.OpenDoor();

    }
    public void Close()
    {
        _door.CloseDoor();
    }
}
