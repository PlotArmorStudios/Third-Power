using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestDoor : MonoBehaviour
{
    [SerializeField] private DoorManager _door;
    [SerializeField] private float _doorOpenTime;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CubeController>())
        {
            FunctionTimer.Create(Open, 0);
            FunctionTimer.Create(Close, _doorOpenTime);
        }
    }
    private void Open()
    {
        _door.OpenDoor();
    }
    private void Close()
    {
        _door.CloseDoor();
    }
}
