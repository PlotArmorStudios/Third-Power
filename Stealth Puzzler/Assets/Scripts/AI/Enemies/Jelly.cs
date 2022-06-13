using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Jelly : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private WolfAI wolfAI;
    private void OnCollisionEnter(Collision collision)
    {
        wolfAI.enabled = true;
        _navMeshAgent.enabled = true;
    }
}
