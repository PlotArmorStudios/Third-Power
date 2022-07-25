using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConfineCam : MonoBehaviour
{
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Colliding");
        var contact = collision.GetContact(0);
        var vector = contact.point.normalized;
        
    }
}
