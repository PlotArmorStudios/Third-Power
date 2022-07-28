using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorManager : Obstacle
{
    private Animator _animator;
    [SerializeField] private bool _openOnLoadSave = true;

    private bool _isOpen { get; set; }

    private void Start()
    {
        _animator = GetComponent<Animator>();

        if (GameManager.Instance.ObstacleBooleans.ContainsKey(_obstacleID) && _openOnLoadSave)
            OpenDoor();
    }

    [ContextMenu("Open Door")]
    public void OpenDoor()
    {
        if (_isOpen) return;
        
        _animator.ResetTrigger("Close");
        _animator.SetTrigger("Open");
        _isOpen = true;

        PlayOpenDoorSound();

        if (GameManager.Instance.ObstacleBooleans.ContainsKey(_obstacleID)) return;
        GameManager.Instance.AddObstacleBoolean(_obstacleID, _isOpen);
    }

    public void OpenDoorNoSave()
    {
        if (_isOpen) return;
        
        _animator.ResetTrigger("Close");
        _animator.SetTrigger("Open");
        _isOpen = true;

        PlayOpenDoorSound();
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