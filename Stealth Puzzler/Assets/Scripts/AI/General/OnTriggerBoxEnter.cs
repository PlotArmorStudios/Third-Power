using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerBoxEnter : MonoBehaviour
{
    private int _numTimesToFlip = 1;
    [SerializeField] private GameObject _jelly;
    [SerializeField] private Rigidbody _jellyRb;
    [SerializeField] private Transform _player;
    bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            triggered = true;
            _jellyRb.useGravity = true;
            if (_numTimesToFlip > 0)
            {
                //_jellyRb.MoveRotation(Quaternion.Inverse(Quaternion.identity));
                _numTimesToFlip--;
            }
            //_jellyRb.MovePosition(_player.position);
        }
    }

    public float ChaseSpeed = 1.0f;
    public float RotateSpeed = 1.0f;
    private void Update()
    {
        if (triggered)
        {
            var step = ChaseSpeed * Time.deltaTime; // calculate distance to move
            _jelly.transform.position = Vector3.MoveTowards(_jelly.transform.position, _player.position, step);
            _jelly.transform.LookAt(_player);
        }
    }
}
