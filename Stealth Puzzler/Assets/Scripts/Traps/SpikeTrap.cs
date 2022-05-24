using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SpikeTrap : MonoBehaviour
{
    private Animator _spikeAnim;
    [SerializeField] private float _graceTimeBeforeFirstAttack = 0.2f;
    [SerializeField] private float _graceTimeBeforeReattack;
    [SerializeField] private float _graceTimeBeforeReattackReset = 2f;
    [SerializeField] private float _timeBeforeRetract = 1f;
    [SerializeField] private float _damageAmount = 1f;
    public static event Action<float> OnDamageTaken;
    void Start()
    {
        _spikeAnim = GetComponent<Animator>();
        _spikeAnim.SetTrigger("retract");
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _graceTimeBeforeReattack = _graceTimeBeforeReattackReset;
            StartCoroutine(PrepAndAttack());
        }
    }
    IEnumerator PrepAndAttack()
    {
        _spikeAnim.SetTrigger("prep");
        yield return new WaitForSeconds(_graceTimeBeforeFirstAttack);
        _spikeAnim.SetTrigger("extend");
        OnDamageTaken?.Invoke(_damageAmount);
        print("damage!");
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
}
