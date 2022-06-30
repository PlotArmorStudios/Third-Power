using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FieldOfView : MonoBehaviour
{
    /// <summary>
    /// Backing value for CanSeePlayer. Use CanSeePlayer instead.
    /// </summary>
    private bool csp;
    [SerializeField] private LayerMask _targetMask;
    public float Radius;
    [Range(0, 360)]
    public float Angle;

    public PlayerController Target { get; private set; }

    public LayerMask ObstructionMask;

    public bool CanSeePlayer 
    {
        get 
        {
            return csp; 
        }
        set 
        {
            if (value != csp)
            {
                if (!csp && value)
                {
                    csp = value;
                    JustSawPlayer.Invoke();
                }
                else
                {
                    csp = value;
                }
            }
        }
    }
    
    public UnityEvent JustSawPlayer;

    private void Start()
    {
        Target = FindObjectOfType<PlayerController>();
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, Radius, _targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < Angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ObstructionMask) && ControllerManager.Instance.PlayerIsActive)
                    CanSeePlayer = true;
                else
                    CanSeePlayer = false;
            }
            else
                CanSeePlayer = false;
        }
        else if (CanSeePlayer)
            CanSeePlayer = false;
    }
}