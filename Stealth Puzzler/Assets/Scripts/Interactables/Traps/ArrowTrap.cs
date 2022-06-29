using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private GameObject _arrow;
    [SerializeField] private Transform _spawnPoint;

    [Tooltip("The delay between shots")] public float SpawnDelay;

    [Tooltip("The delay between the start of the level and the first shot")]
    public float InitialDelay = 0;

    public bool IsActive;

    private float _currentSpawnTime;

    private void Start()
    {
        _currentSpawnTime = 0;
        
        if (InitialDelay > 0)
        {
            IsActive = false;
            Invoke("Activate", InitialDelay);
        }
    }

    private void Update()
    {
        if (!IsActive) return;

        _currentSpawnTime += Time.deltaTime;

        if (_currentSpawnTime >= SpawnDelay)
        {
            StopCoroutine(ShootArrow());
            StartCoroutine(ShootArrow());
            _currentSpawnTime = 0;
        }
    }

    [ContextMenu("Activate")]
    public void Activate()
    {
        IsActive = true;
    }

    [ContextMenu("Deactivate")]
    public void Deactivate()
    {
        IsActive = false;
    }

    private IEnumerator ShootArrow()
    {
        PlayReloadSound();

        yield return new WaitForSeconds(2f);

        var arrow = Instantiate(_arrow, _spawnPoint.position, transform.rotation);
        arrow.transform.parent = transform;

        PlayShootSound();
    }

    [ContextMenu("Shoot Arrow No Reload")]
    public void ShootArrowNoReload()
    {
        var arrow = Instantiate(_arrow, _spawnPoint.position, transform.rotation);
        arrow.transform.parent = transform;

        //PlayShootSound();
    }
    private void PlayReloadSound()
    {
        //Implement arrow reload sound here
        Debug.Log("Play reload sound");
        AkSoundEngine.PostEvent("Play_Trap_Reload", gameObject);
    }

    private void PlayShootSound()
    {
        //Implement arrow shoot sound here
        AkSoundEngine.PostEvent("Play_Cannon", gameObject);
    }
}