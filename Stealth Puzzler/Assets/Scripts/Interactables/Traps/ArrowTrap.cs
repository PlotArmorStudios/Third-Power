using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private GameObject _arrow;
    [SerializeField] private Transform _spawnPoint;
    
    public float SpawnDelay;
    
    public bool IsActive;
    
    private float _currentSpawnTime;

    private void Update()
    {
        if (!IsActive) return;
        
        _currentSpawnTime += Time.deltaTime;

        if (_currentSpawnTime >= SpawnDelay)
        {
            ShootArrow();
        }
    }
    
    [ContextMenu("Activate")]
    public void Activate()
    {
        IsActive = true;
        _currentSpawnTime = SpawnDelay;
    }

    [ContextMenu("Deactivate")]
    public void Deactivate()
    {
        IsActive = false;
    }
    
    private void ShootArrow()
    {
        var arrow = Instantiate(_arrow, _spawnPoint.position, transform.rotation);
        _currentSpawnTime = 0;
        arrow.transform.parent = transform;

        PlayShootSound();
    }

    private void PlayShootSound()
    {
        //Implement arrow shoot sound here
    }
}