using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapIdleLoadAttack : MonoBehaviour
{
    private Animator _spikeAnim;
    [SerializeField] private float _graceTimeBeforeReattack;
    [SerializeField] private float _graceTimeBeforeReattackReset = 2f;
    [SerializeField] private float _reloadTime = 1f;
    void Start()
    {
        _spikeAnim = GetComponent<Animator>();
        _spikeAnim.SetTrigger("idle");
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
        _spikeAnim.SetTrigger("load");
        _spikeAnim.SetTrigger("attack");
        yield return new WaitForSeconds(_reloadTime);
        _spikeAnim.SetTrigger("idle");
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
