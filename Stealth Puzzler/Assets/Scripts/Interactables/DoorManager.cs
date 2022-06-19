using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : Obstacle
{
    private Animator _animator;

    private bool _isOpen { get; set; }

    private void Start()
    {
        _animator = GetComponent<Animator>();
        
        if (GameManager.Instance.ObstacleBooleans[_obstacleID])
            OpenDoor();
    }

    public void OpenDoor()
    {
        _animator.SetTrigger("Open");
        _isOpen = true;
        
        if (GameManager.Instance.ObstacleBooleans.ContainsKey(_obstacleID)) return;
        GameManager.Instance.AddObstacleBoolean(_obstacleID, _isOpen);

        PlayOpenDoorSound();
    }

    private void PlayOpenDoorSound()
    {
        //Implement open door sound here
    }
}