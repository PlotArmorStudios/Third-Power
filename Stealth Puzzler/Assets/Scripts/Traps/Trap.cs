using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Trap : MonoBehaviour
{
    private Animator _trapAnim;
    [SerializeField] private TrapState _trapState = TrapState.Waiting;
    [SerializeField] private float _graceTimeBeforeAttack = 2f;
    [SerializeField] private float _timeBeforeRetractWhenAway = 1f;
    private float _timeToStayAttacking = 1f;
    [SerializeField] private float _timeBeforeDamageReoccursWhenAttacked = 3f;
    private float _timeAfterAttackToDamage = 0.3f;
    [SerializeField] private float _damageAmount = 1f;
    public static event Action<float> OnDamageTaken;
    private bool isInTriggerZone;

    // Start is called before the first frame update
    void Start()
    {
        _trapAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        switch (_trapState)
        {
            case TrapState.Waiting:
                ResetAllTriggers();
                _trapAnim.SetTrigger("retract");
                break;
            case TrapState.PrepToAttack:
                ResetAllTriggers();
                StartCoroutine(PrepAndAttack());
                break;
            case TrapState.Attacking:
                ResetAllTriggers();
                _trapAnim.SetTrigger("extend");
                break;
            case TrapState.Resetting:
                ResetAllTriggers();
                _trapAnim.SetTrigger("retract");
                break;
            case TrapState.Deactivated:
                ResetAllTriggers();
                _trapAnim.SetTrigger("retract");
                break;
            default:
                break;
        }
    }
    private void ResetAllTriggers()
    {
        _trapAnim.ResetTrigger("retract");
        _trapAnim.ResetTrigger("prep");
        _trapAnim.ResetTrigger("extend");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _trapState = TrapState.PrepToAttack;
            isInTriggerZone = true;
        }
    }

    IEnumerator PrepAndAttack()
    {
        _trapAnim.SetTrigger("prep");
        yield return new WaitForSeconds(_graceTimeBeforeAttack);
        _trapState = TrapState.Attacking;
        yield return new WaitForSeconds(_timeBeforeRetractWhenAway);
        if (!isInTriggerZone) _trapState = TrapState.Waiting;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(OnTriggerExited());
            isInTriggerZone = false;
            _timeAfterAttackToDamage = 0.3f;
        }
    }

    IEnumerator OnTriggerExited()
    {
        yield return new WaitForSeconds(_timeBeforeRetractWhenAway);
        _trapState = TrapState.Waiting;
    }

    private void OnTriggerStay(Collider other)
    {
        if (_trapState != TrapState.Attacking) return;
        _timeAfterAttackToDamage -= Time.deltaTime;
        if (_timeAfterAttackToDamage > 0) return;

        if (other.CompareTag("Player"))
        {
            OnDamageTaken?.Invoke(_damageAmount);
            _timeAfterAttackToDamage = _timeBeforeDamageReoccursWhenAttacked;
            print("damage!");
        }
    }
}
