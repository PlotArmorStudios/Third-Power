using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class OnTrapHit : MonoBehaviour
{
    [SerializeField] private float _damageAmount = 1f;
    public static event Action<float> OnDamageTaken;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            OnDamageTaken?.Invoke(_damageAmount);
            print("damage!");
        }
    }
}
