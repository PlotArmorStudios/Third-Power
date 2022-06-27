using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class WayPoints : MonoBehaviour
{
    [Range(0f, 2f)] [SerializeField] private float _wayPointSize = 1f;
    private Func<Transform, int> getNext = w => w.GetSiblingIndex() + 1;


    public Transform GetNextWayPoint(Transform currentWaypoint, WaypointObject.Mode mode = WaypointObject.Mode.Loop)
    {
        if (currentWaypoint == null)
        {
            return transform.GetChild(0);
        }
        int currentIndex = getNext(currentWaypoint);
        if (currentIndex < transform.childCount &&
            currentIndex >= 0)
        {
            return transform.GetChild(currentIndex);
        }
        else
            switch (mode)
            {
                case WaypointObject.Mode.Loop:
                    getNext = (w) => w.GetSiblingIndex() + 1;
                    return transform.GetChild(0);
                case WaypointObject.Mode.PingPong:
                    getNext = (w) => w.GetSiblingIndex() - 1;
                    return GetNextWayPoint(currentWaypoint, mode);
                case WaypointObject.Mode.Once:
                    return currentWaypoint;
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