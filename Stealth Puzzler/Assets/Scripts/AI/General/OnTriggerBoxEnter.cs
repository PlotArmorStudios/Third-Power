using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnTriggerBoxEnter : MonoBehaviour
{
    [SerializeField] private Rigidbody _jellyRb;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            _jellyRb.useGravity = true;
        }
    }
}
