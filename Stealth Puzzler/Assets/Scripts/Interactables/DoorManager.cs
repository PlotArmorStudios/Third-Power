using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : MonoBehaviour
{
    [SerializeField] private string _obstacleID;
    private Animator _animator;

    public bool IsOpen { get; set; }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        if (GameManager.Instance.ObstacleBooleans[_obstacleID])
            OpenDoor();
    }

    public void OpenDoor()
    {
        _animator.SetTrigger("Open");
        IsOpen = true;
        GameManager.Instance.AddObstacleBoolean(_obstacleID, IsOpen);
    }
}