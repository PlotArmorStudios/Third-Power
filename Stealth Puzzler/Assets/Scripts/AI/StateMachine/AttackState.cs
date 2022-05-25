using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class AttackState : ScriptableObject, IState
{
    public void OnEnter()
    {
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public void UpdateState()
    {
        Debug.Log("I have attacked");
    }
}
