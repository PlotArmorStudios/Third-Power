using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class JellyTriggerBox : MonoBehaviour
{
    [SerializeField] private GameObject _jellyController;
    [SerializeField] private Rigidbody _jellyRb;
    [SerializeField] private float _initialForceMultiplier;
    [SerializeField] private ControllerManager _player;
    [SerializeField] private Controller _playerCube;
    [SerializeField] private Controller _playerHumanoid;

    private void Start()
    {
        _player = FindObjectOfType<ControllerManager>(true);
        _playerCube = FindObjectOfType<CubeController>(true);
        _playerHumanoid = FindObjectOfType<PlayerController>(true);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (GameObject.ReferenceEquals(other.gameObject, _playerCube.gameObject))
        {
            print("cube hit");
            _jellyRb.useGravity = true;
            _jellyRb.AddForce(Vector3.down * _initialForceMultiplier);
            
        }
        else if (GameObject.ReferenceEquals(other.gameObject, _playerHumanoid.gameObject))
        {
            print("humanoid hit");
            _jellyRb.useGravity = true;
            _jellyRb.AddForce(Vector3.down * _initialForceMultiplier);
        }
    }
}
