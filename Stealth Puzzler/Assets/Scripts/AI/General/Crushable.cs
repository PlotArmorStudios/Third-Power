using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crushable : MonoBehaviour
{
    [SerializeField] private float _deactivationDelay;
    
    private Animator _animator;

    private void Start()
    {
        _animator = GetComponentInChildren<Animator>();
    }

    public void Crush()
    {
        _animator.SetTrigger("Crush");
        StartCoroutine(DeactiveAfterSquash());
    }

    public IEnumerator DeactiveAfterSquash()
    {
        yield return new WaitForSeconds(_deactivationDelay);
        gameObject.SetActive(false);
    }
}
