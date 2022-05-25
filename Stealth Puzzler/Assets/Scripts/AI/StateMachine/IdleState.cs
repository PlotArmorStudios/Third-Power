using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class IdleState : IState
{
    public ChaseState ChaseState;
    public PatrolState PatrolState;
    public bool CanSeePlayer;
    private float _randomIdleTime;
    [SerializeField] private float _minIdleTime = 1f;
    [SerializeField] private float _maxFloatTime = 3f;

    public void OnEnter()
    {
        //StateType = this;
        _randomIdleTime = Random.Range(_minIdleTime, _maxFloatTime);
    }

    public void OnExit()
    {

    }

    public void UpdateState()
    {
        _randomIdleTime -= Time.deltaTime;
        if (CanSeePlayer)
        {
            //StateType = ChaseState;
        }
        else if (_randomIdleTime < 0)
        {
            //StateType = PatrolState;
        }
        else
        {
            //StateType = this;
        }
    }
}