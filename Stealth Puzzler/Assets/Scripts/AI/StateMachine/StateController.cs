using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateController : MonoBehaviour
{
    [SerializeField] private IdleState _idleState;
    [SerializeField] private PatrolState _patrolState;
    [SerializeField] private ChaseState _chaseState;
    [SerializeField] private AttackState _attackState;
    public IState CurrentState;


    void Update()
    {
        RunStateMachine();
    }

    private void RunStateMachine()
    {
        IState nextState = CurrentState;

        if (nextState != null)
        {   
            SwitchToTheNextState(nextState);
        }
    }

    private void SwitchToTheNextState(IState nextState)
    {
        CurrentState = nextState;
    }
}
