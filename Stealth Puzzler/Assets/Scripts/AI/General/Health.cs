using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Health : MonoBehaviour
{
    public event Action OnDie;

    [SerializeField] private int _maxNumberOfHits;

    private int _currentNumberOfHits;

    private void Start()
    {
        _currentNumberOfHits = _maxNumberOfHits;
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
        OnDie?.Invoke();
    }
}