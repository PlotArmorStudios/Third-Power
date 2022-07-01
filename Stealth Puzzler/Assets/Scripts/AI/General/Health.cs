using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    public event Action OnDie;

    [SerializeField] private int _maxNumberOfHits;

    private int _currentNumberOfHits;
    protected Animator _animator;
    
    protected virtual void Start()
    {
        _currentNumberOfHits = _maxNumberOfHits;
        _animator = GetComponent<Animator>();
    }

    public virtual void TakeHit()
    {
        _currentNumberOfHits--;
        if (_currentNumberOfHits <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        gameObject.SetActive(false);
    }

    protected void TriggerOnDie()
    {
        _animator.SetTrigger("Die");
        OnDie?.Invoke();
    }
}