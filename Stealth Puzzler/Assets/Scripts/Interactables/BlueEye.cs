using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BlueEye : MonoBehaviour
{
    [SerializeField] private UnityEvent _collisionEvent;

    private void OnCollisionEnter(Collision other)
    {
        var projectile = other.gameObject.GetComponent<Projectile>();
        if (!projectile) return;
        _collisionEvent?.Invoke();
    }
}