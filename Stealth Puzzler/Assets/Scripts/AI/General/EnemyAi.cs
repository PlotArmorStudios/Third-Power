using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class EnemyAi : MonoBehaviour
{
    private NavMeshAgent _navAgent;
    public GameObject Player;
    [SerializeField] private FieldOfView _fieldOfView;
    [SerializeField] private State _currentState;
    private Rigidbody _rb;

    // Start is called before the first frame update
    void Start()
    {
        _navAgent = this.GetComponent<NavMeshAgent>();
        _currentState = State.Idle;
        _timeToStayPatrolling = RandomTime(_minTimeToPatrol, _maxTimeToPatrol);
        _timeToStayIdle = RandomTime(_minTimeToStayIdle, _maxTimeToStayIdle);
        _rb = GetComponent<Rigidbody>();
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
                WindUpIfCanSeePlayer();
                break;
            case State.Patrol:
                Patrol();
                WindUpIfCanSeePlayer();
                break;
            case State.WindUp:
                WindUp();
                break;
            case State.Chase:
                Chase(Player.transform.position);
                break;
            case State.Attack:
                break;
            default:
                break;
        }
    }

    private void WindUpIfCanSeePlayer()
    {
        if (_fieldOfView.CanSeePlayer)
        {
            _currentState = State.WindUp;
        }
    }

    [SerializeField] private float _minTimeToStayIdle;
    [SerializeField] private float _maxTimeToStayIdle;
    private float _timeToStayIdle;
    void Idle()
    {
        _timeToStayIdle -= Time.deltaTime;
        _navAgent.ResetPath();
        _rb.velocity = Vector3.zero;
        _rb.rotation = Quaternion.identity;
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
    [SerializeField] private float _patrolSpeed = 3f;
    [SerializeField] private float _patrolAcceleration = 8f;
    void Patrol()
    {
        _navAgent.acceleration = _patrolAcceleration;
        _navAgent.speed = _patrolSpeed;
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
        _navAgent.SetDestination(location);
    }

    [SerializeField] private float _chaseSpeed = 5f;
    [SerializeField] private float _chaseAcceleration = 12f;
    void Chase(Vector3 location)
    {
        if (_fieldOfView.CanSeePlayer)
        {
            _navAgent.SetDestination(location);
            Quaternion.LookRotation(Player.transform.position - transform.position);
            //Quaternion.FromToRotation(transform.forward, Player.transform.position - transform.position);
            //transform.LookAt(target.transform.position);
            _navAgent.speed = _chaseSpeed;
            _navAgent.acceleration = _chaseAcceleration;
        }
        else
        {
            _currentState = State.Idle;
        }
    }

    [SerializeField] private float _resetWindUpTime = .5f;
    [SerializeField] private float _windUpTime = .5f;
    void WindUp()
    {
        _navAgent.ResetPath();
        Quaternion.LookRotation(Player.transform.position - transform.position);
        _windUpTime -= Time.deltaTime;
        if (_fieldOfView.CanSeePlayer && _windUpTime<0)
        {
            _windUpTime = _resetWindUpTime;
            _currentState = State.Chase;
        }
        else
        {
            _currentState = State.Idle;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _rb.velocity = Vector3.zero;
            _rb.rotation = Quaternion.identity;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            _rb.velocity = Vector3.zero;
            _rb.rotation = Quaternion.identity;
        }
    }

    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        _navAgent.enabled = false;
    //        _rb.isKinematic = false;
    //        _rb.useGravity = true;
    //        Invoke(nameof(EnableNavMesh), 1F);
    //    }
    //}

    //void EnableNavMesh()
    //{
    //    _rb.isKinematic = true;
    //    _rb.useGravity = false;
    //    _navAgent.enabled = true;
    //}

    //void Flee(Vector3 location)
    //{
    //    Vector3 fleeVector = location - this.transform.position;
    //    agent.SetDestination(this.transform.position - fleeVector);
    //}
}