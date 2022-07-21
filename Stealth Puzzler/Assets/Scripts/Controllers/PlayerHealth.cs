using System;
using System.Collections;
using Helpers;
using UnityEngine;

public class PlayerHealth : Health
{
    public static event Action OnPlayerDie;
    [SerializeField] private float _levelRestartDelay = 2f;

    private IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(_levelRestartDelay);
        SceneLoader.Instance.RestartLevel();
    }

    protected override void Die()
    {
        _dead = true;
        TriggerOnDie();
        OnPlayerDie?.Invoke();
        ControllerManager.Instance.DeactivatePlayer();
        StartCoroutine(RestartLevel());
        CamRig.Instance.UnsetFollow();
        AkSoundEngine.PostEvent("Play_player_voice_death", gameObject);
        AkSoundEngine.PostEvent("stop_puzzle_time_running_out", gameObject);
    }
}