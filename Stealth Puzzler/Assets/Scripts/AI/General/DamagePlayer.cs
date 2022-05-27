using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        var player = other.gameObject.GetComponent<PlayerController>();
        if (!player) return;
        player.GetComponent<Health>().TakeHit();
    }
}
