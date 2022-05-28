using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    [Range(0f, 2f)] [SerializeField] private float _wayPointSize = 1f;


    public Transform GetNextWayPoint(Transform currenWaypoint)
    {
        if (currenWaypoint == null)
        {
            return transform.GetChild(0);
        }

        if (currenWaypoint.GetSiblingIndex() < transform.childCount - 1)
        {
            return transform.GetChild(currenWaypoint.GetSiblingIndex() + 1);
        }
        else
        {
            return transform.GetChild(0);
        }
        return null;
    }
    
    private void OnDrawGizmos()
    {
        foreach (Transform t in transform)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(t.position, _wayPointSize);
        }

        Gizmos.color = Color.red;
        for (int i = 0; i < transform.childCount - 1; i++)
        {
            Gizmos.DrawLine(transform.GetChild(i).position, transform.GetChild(i + 1).position);
        }

        Gizmos.DrawLine(transform.GetChild(transform.childCount - 1).position, transform.GetChild(0).position);
    }
}