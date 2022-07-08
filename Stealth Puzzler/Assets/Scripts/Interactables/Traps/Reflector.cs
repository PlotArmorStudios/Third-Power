using System;
using UnityEngine;

public class Reflector : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        
        Gizmos.DrawRay(transform.position, transform.forward * 2f);
    }
}