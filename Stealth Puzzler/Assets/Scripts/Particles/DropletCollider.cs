using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropletCollider : MonoBehaviour
{
    private void OnParticleCollision(GameObject other)
    {
        Debug.Log("collide");
    }
}
