#define DebugStates
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class WolfAI : MonoBehaviour
{
    [SerializeField] private FieldOfView _fieldOfView;
    [SerializeField] private State _currentState;
    [SerializeField] private Animator _animator;

    private LayerMask _enemyLayer;
    private LayerMask _playerLayer;

    private NavMeshAgent _navAgent;
    private Controller _player;
    private Rigidbody _rb;

    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _currentState = State.Idle;
        _timeToStayPatrolling = RandomTime(_minTimeToPatrol, _maxTimeToPatrol);
        _timeToStayIdle = RandomTime(_minTimeToStayIdle, _maxTimeToStayIdle);
        _rb = GetComponent<Rigidbody>();
        _player = FindObjectOfType<Controller>(true);
    }

    void Update()
    {
        SwitchStates();
        Debug.Log(_player.transform.position);
    }

    private void SwitchStates()
    {
        switch (_currentState)
        {
            case State.Idle:
#if DebugStates
                Debug.Log("Ticking Idle");
#endif
                Idle();
                WindUpIfCanSeePlayer();
                break;
            case State.Patrol:
#if DebugStates
                Debug.Log("Ticking Patrol");
#endif
                Patrol();
                WindUpIfCanSeePlayer();
                break;
            case State.WindUp:
#if DebugStates
                Debug.Log("Ticking WindUp");
#endif
                WindUp();
                break;
            case State.Chase:
#if DebugStates
                Debug.Log("Ticking Chase");
#endif
                Chase(_player.transform.position);
                break;
            case State.Attack:
#if DebugStates
                Debug.Log("Ticking Attack");
#endif
                break;
            case State.TurnAround:
#if DebugStates
                Debug.Log("Ticking Turn Around");
#endif
                RevertDirection();
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
        _animator.SetBool("Running", true);
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
        Vector3 targetWorld = gameObject.transform.InverseTransformVector(targetLocal);

        Seek(targetWorld);
        if (_timeToStayPatrolling < 0)
        {
            _timeToStayPatrolling = RandomTime(_minTimeToPatrol, _maxTimeToPatrol);
            _animator.SetBool("Running", false);
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
    [SerializeField] private float _chaseDistance = 5f;

    void Chase(Vector3 location)
    {
        if (_fieldOfView.CanSeePlayer)
        {
            var targetDirection = (location - _rb.transform.position).normalized;
            var targetPosition = location + (targetDirection * _chaseDistance);
            _rb.transform.rotation = Quaternion.LookRotation(_player.transform.position - _rb.transform.position);
            _navAgent.SetDestination(targetPosition);
            _navAgent.speed = _chaseSpeed;
            _navAgent.acceleration = _chaseAcceleration;
        }
        else
        {
            _currentState = State.TurnAround;
        }
    }

    private void RevertDirection()
    {
        _navAgent.ResetPath();
        _rb.transform.rotation = Quaternion.LookRotation(_player.transform.position - _rb.transform.position);
        
        WindUpIfCanSeePlayer();
    }

    private IEnumerator TurnToPlayer()
    {
        yield return null;
    }

    [SerializeField] private float _resetWindUpTime = .5f;
    [SerializeField] private float _windUpTime = .5f;


    void WindUp()
    {
        _navAgent.ResetPath();
        _animator.SetBool("Running", false);
        _rb.transform.rotation = Quaternion.LookRotation(_player.transform.position - transform.position);
        _windUpTime -= Time.deltaTime;
        if (_fieldOfView.CanSeePlayer && _windUpTime < 0)
        {
            _windUpTime = _resetWindUpTime;
            _currentState = State.Chase;
            _animator.SetTrigger("Chase");
        }
        else
        {
            _currentState = State.Idle;
        }
    }
}