using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private Projectile _arrow;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _arrowSpeed;
    
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
            Shoot();
            _currentSpawnTime = 0;
        }
    }

    private void Shoot()
    {
        StopCoroutine(ShootArrow());
        StartCoroutine(ShootArrow());
    }

    private void Shoot(float shootSpeed)
    {
        StopCoroutine(ShootArrow(shootSpeed));
        StartCoroutine(ShootArrow(shootSpeed));
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
        arrow.SetSpeed(_arrowSpeed);
        
        PlayShootSound();
    }
    
    private IEnumerator ShootArrow(float shootSpeed)
    {
        PlayReloadSound();

        yield return new WaitForSeconds(2f);

        var arrow = Instantiate(_arrow, _spawnPoint.position, transform.rotation);
        arrow.SetSpeed(shootSpeed);
        
        PlayShootSound();
    }
    
    [ContextMenu("Shoot Arrow No Reload")]
    public void ShootArrowNoReload()
    {
        var arrow = Instantiate(_arrow, _spawnPoint.position, transform.rotation);
    }

    [ContextMenu("Single Shot")]
    public void SingleShot()
    {
        Shoot();
    }

    public void SingleShot(float shootSpeed)
    {
        Shoot(shootSpeed);
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