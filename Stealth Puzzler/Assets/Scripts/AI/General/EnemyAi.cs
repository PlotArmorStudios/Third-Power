using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAi : MonoBehaviour
{
    NavMeshAgent agent;
    public GameObject target;
    [SerializeField] private FieldOfView FieldOfView;
    [SerializeField] private State _currentState;

    // Start is called before the first frame update
    void Start()
    {
        agent = this.GetComponent<NavMeshAgent>();
        _currentState = State.Idle;
        _timeToStayPatrolling = RandomTime(_minTimeToPatrol, _maxTimeToPatrol);
        _timeToStayIdle = RandomTime(_minTimeToStayIdle, _maxTimeToStayIdle);
    }
    void Update()
    {
        SwitchStates();
    }

    private void SwitchStates()
    {
        switch (_currentState)
        {
            case State.Idle:
                Idle();
                ChaseIfCanSeePlayer();
                break;
            case State.Patrol:
                Patrol();
                ChaseIfCanSeePlayer();
                break;
            case State.Chase:
                Chase(target.transform.position);
                Chase(target.transform.position);
                break;
            case State.Attack:
                break;
            default:
                break;
        }
    }

    private void ChaseIfCanSeePlayer()
    {
        if (FieldOfView.CanSeePlayer)
        {
            _currentState = State.Chase;
        }
    }

    [SerializeField] private float _minTimeToStayIdle;
    [SerializeField] private float _maxTimeToStayIdle;
    private float _timeToStayIdle;
    void Idle()
    {
        _timeToStayIdle -= Time.deltaTime;
        agent.SetDestination(transform.position);
        //play idle animation
        if (_timeToStayIdle < 0)
        {
            _timeToStayIdle = RandomTime(_minTimeToStayIdle, _maxTimeToStayIdle);
            _currentState = State.Patrol;
        }
    }

    private float RandomTime(float minTime, float maxTime)
    {
        return UnityEngine.Random.Range(minTime, maxTime);
    }

    [SerializeField] private float _minTimeToPatrol;
    [SerializeField] private float _maxTimeToPatrol;
    private float _timeToStayPatrolling;
    Vector3 wanderTarget = Vector3.zero;
    void Patrol()
    {
        _timeToStayPatrolling -= Time.deltaTime;
        float wanderRadius = 10;
        float wanderDistance = 10;
        float wanderJitter = 1;

        //determine a location on a circle 
        wanderTarget += new Vector3(UnityEngine.Random.Range(-1.0f, 1.0f) * wanderJitter,
                                        0,
                                        UnityEngine.Random.Range(-1.0f, 1.0f) * wanderJitter);
        wanderTarget.Normalize();
        //project the point out to the radius of the cirle
        wanderTarget *= wanderRadius;

        //move the circle out in front of the agent to the wander distance
        Vector3 targetLocal = wanderTarget + new Vector3(0, 0, wanderDistance);
        //work out the world location of the point on the circle.
        Vector3 targetWorld = this.gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
        if (_timeToStayPatrolling < 0)
        {
            _timeToStayPatrolling = RandomTime(_minTimeToPatrol, _maxTimeToPatrol);
            _currentState = State.Idle;
        }
        
    }
    //send agent to a location on the nav mesh
    void Seek(Vector3 location)
    {
        agent.SetDestination(location);
    }

    void Chase(Vector3 location)
    {
        if (FieldOfView.CanSeePlayer)
        {
            agent.SetDestination(location);
            transform.LookAt(target.transform);
        }
        else
        {
            _currentState = State.Idle;
        }
    }



    //void Flee(Vector3 location)
    //{
    //    Vector3 fleeVector = location - this.transform.position;
    //    agent.SetDestination(this.transform.position - fleeVector);
    //}
}