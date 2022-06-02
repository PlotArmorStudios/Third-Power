using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Jelly : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private EnemyAi enemyAi;
    private void OnCollisionEnter(Collision collision)
    {
        enemyAi.enabled = true;
        navMeshAgent.enabled = true;
    }
}
