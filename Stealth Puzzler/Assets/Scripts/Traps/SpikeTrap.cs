using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpikeTrap : MonoBehaviour
{
    private Animator _spikeAnim;
    [SerializeField] private float _graceTimeBeforeFirstAttack = 0.5f;
    [SerializeField] private float _graceTimeBeforeReattack;
    [SerializeField] private float _graceTimeBeforeReattackReset = 2f;
    [SerializeField] private float _timeBeforeRetract = 1f;
    [SerializeField] private float _damageAmount = 1f;
    public static event Action<float> OnDamageTaken;
    private bool _isInTriggerZone = false;
    void Start()
    {
        _spikeAnim = GetComponent<Animator>();
        _spikeAnim.SetTrigger("retract");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _isInTriggerZone = true;
            _graceTimeBeforeReattack = _graceTimeBeforeReattackReset;
            StartCoroutine(PrepAndAttack());
        }
    }
    IEnumerator PrepAndAttack()
    {
        _spikeAnim.SetTrigger("prep");
        _spikeAnim.SetTrigger("extend");
        yield return new WaitForSeconds(_graceTimeBeforeFirstAttack);
        if (_isInTriggerZone)
        {
            OnDamageTaken?.Invoke(_damageAmount);
            print("damage!");
        }
        yield return new WaitForSeconds(_timeBeforeRetract);
        _spikeAnim.SetTrigger("retract");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _graceTimeBeforeReattack -= Time.deltaTime;
        }
        if (_graceTimeBeforeReattack < 0)
        {
            _graceTimeBeforeReattack = _graceTimeBeforeReattackReset;
            StartCoroutine(PrepAndAttack());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _isInTriggerZone = false;
    }
}
