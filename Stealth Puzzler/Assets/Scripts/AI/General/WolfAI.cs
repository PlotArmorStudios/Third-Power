//#define DebugStates
//#define PatrolDebug
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

public class WolfAI : MonoBehaviour
{
    [SerializeField] private FieldOfView _fieldOfView;
    [SerializeField] private State _currentState;
    [SerializeField] private Animator _animator;
    [SerializeField] private bool _togglePatrol = true;
    
    private LayerMask _enemyLayer;
    private LayerMask _playerLayer;

    private NavMeshAgent _navAgent;
    private Controller _player;
    private Rigidbody _rb;

    //Patrolling
    [SerializeField] private float _homeRadius = 5f;
    [SerializeField] private float _minTimeToStayIdle;
    [SerializeField] private float _maxTimeToStayIdle;

    private bool _patrolling;
    private float _timeToStayIdle;
    private float _patrolTime;
    private float _timeToPatrol;
    private Vector3 _newDestination;
    private Vector3 _startPosition;

    //Chase
    [SerializeField] private float _chaseSpeed = 5f;
    [SerializeField] private float _chaseAcceleration = 12f;
    [SerializeField] private float _chaseDistance = 5f;

    [SerializeField] private float _attackRange;
    
    //Wind Up
    [SerializeField] private float _resetWindUpTime = .5f;
    [SerializeField] private float _windUpTime = .5f;

    void Start()
    {
        _navAgent = GetComponent<NavMeshAgent>();
        _currentState = State.Idle;
        _timeToStayIdle = RandomTime(_minTimeToStayIdle, _maxTimeToStayIdle);
        _rb = GetComponent<Rigidbody>();
        _player = FindObjectOfType<Controller>(true);
        _startPosition = transform.position;
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
#if DebugStates
                Debug.Log("Ticking Idle");
#endif
                Idle();
                ChaseIfCanSeePlayer();
                break;
            case State.Patrol:
#if DebugStates
                Debug.Log("Ticking Patrol");
#endif
                PatrolArea();
                ChaseIfCanSeePlayer();
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
                break;
            case State.Attack:
#if DebugStates
                Debug.Log("Ticking Attack");
#endif
                Tackle(_player.transform.position);
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

    private void ChaseIfCanSeePlayer()
    {
        if (_fieldOfView.CanSeePlayer)
        {
            _currentState = State.Chase;
        }
    }

    private void TackleIfInRange()
    {
        
    }

    void Idle()
    {
        _timeToStayIdle -= Time.deltaTime;
        _navAgent.ResetPath();
        _rb.velocity = Vector3.zero;
        _rb.rotation = Quaternion.identity;

        //play idle animation

        if (!_togglePatrol) return;
        
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

    private void PatrolArea()
    {
        if (_patrolling == false)
            _patrolTime += Time.deltaTime;

        if (_patrolTime >= _timeToPatrol)
        {
            _timeToPatrol = Random.Range(2, 12);
            TriggerPatrol();
            _patrolTime = 0;
        }

        if (Vector3.Distance(transform.position, _newDestination) < 1f)
        {
            _animator.SetBool("Running", false);
            _patrolling = false;
        }
    }

    private void TriggerPatrol()
    {
        Vector3 randomDirection = Random.insideUnitSphere * _homeRadius;
        randomDirection += _startPosition;

        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, _homeRadius, 1);
        Vector3 finalPosition = hit.position;
        _newDestination = finalPosition;

#if PatrolDebug
        Debug.Log("Hit position is: " + hit.position);
        Debug.Log("New destination is: " + _newDestination);
#endif
        _navAgent.destination = finalPosition;
        _animator.SetBool("Running", true);
        _patrolling = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, .2f);
        Gizmos.DrawSphere(_newDestination, 2f);
    }

    void Tackle(Vector3 location)
    {
        if (_fieldOfView.CanSeePlayer)
        {
            var targetDirection = (location - _rb.transform.position).normalized;
            var targetPosition = location + (targetDirection * _chaseDistance);
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

        _currentState = State.Idle;
    }

    void WindUp()
    {
        _navAgent.ResetPath();
        _rb.transform.rotation = Quaternion.LookRotation(_player.transform.position - _rb.transform.position);
        _animator.SetBool("Running", false);
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