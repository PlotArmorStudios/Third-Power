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

        if (GameManager.Instance.ObstacleBooleans.ContainsKey(_obstacleID))
            OpenDoor();
    }

    [ContextMenu("Open Door")]
    public void OpenDoor()
    {
        _animator.ResetTrigger("Close");
        _animator.SetTrigger("Open");
        _isOpen = true;

        PlayOpenDoorSound();

        if (GameManager.Instance.ObstacleBooleans.ContainsKey(_obstacleID)) return;
        GameManager.Instance.AddObstacleBoolean(_obstacleID, _isOpen);
    }

    private void PlayOpenDoorSound()
    {
        //Implement open door sound here
        AkSoundEngine.PostEvent("Play_door_open", gameObject);
    }

    public void CloseDoor()
    {
        _animator.ResetTrigger("Open");
        _animator.SetTrigger("Close");
        _isOpen = false;

        PlayCloseDoorSound();

        if (GameManager.Instance.ObstacleBooleans.ContainsKey(_obstacleID)) return;
        GameManager.Instance.AddObstacleBoolean(_obstacleID, _isOpen);
    }
    
    private void PlayCloseDoorSound()
    {
        //Implement close door sound here
    }
}