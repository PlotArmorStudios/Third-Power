using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;

public class Death : MonoBehaviour
{
    public event Action OnDie;
    [SerializeField] private float _levelRestartDelay = 2f;

    public void Die()
    {
        OnDie?.Invoke();
        StartCoroutine(RestartLevel());
        //gameObject.SetActive(false);
    }

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(_levelRestartDelay);
        SceneLoader.Instance.RestartLevel();
    }
}