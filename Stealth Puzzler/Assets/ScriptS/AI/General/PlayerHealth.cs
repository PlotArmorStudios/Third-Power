using System.Collections;
using UnityEngine;

public class PlayerHealth : Health
{
    [SerializeField] private float _levelRestartDelay = 2f;
    
    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(_levelRestartDelay);
        SceneLoader.Instance.RestartLevel();
    }

    protected override void Die()
    {
        TriggerOnDie();
        StartCoroutine(RestartLevel());
    }
}