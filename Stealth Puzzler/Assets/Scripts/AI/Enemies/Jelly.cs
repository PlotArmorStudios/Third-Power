using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Jelly : MonoBehaviour
{
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private EnemyAi _enemyAi;
    private void OnCollisionEnter(Collision collision)
    {
        _enemyAi.enabled = true;
        _navMeshAgent.enabled = true;
    }
}
