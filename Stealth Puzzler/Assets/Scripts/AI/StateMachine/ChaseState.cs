using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class ChaseState : IState
{
    public AttackState AttackState;
    public bool IsInAttackRange;

    public void OnEnter()
    {
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateState()
    {
        if (IsInAttackRange)
        {
            //return AttackState;
        }
        else
        {
            //return this;
        }
        
    }
}
