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
    public static event Action OnJellyTriggerBoxEntered;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<CubeController>())
        {
            _jellyRb.useGravity = true;
            _jellyRb.AddForce(Vector3.down * _initialForceMultiplier);
        }
        else if (other.gameObject.GetComponent<PlayerController>())
        {
            _jellyRb.useGravity = true;
            _jellyRb.AddForce(Vector3.down * _initialForceMultiplier);
        }
    }
}
