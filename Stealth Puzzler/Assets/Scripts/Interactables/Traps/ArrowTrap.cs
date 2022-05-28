using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] private GameObject _arrow;
    [SerializeField] private Transform _spawnPoint;
    
    public float SpawnDelay;
    private float _currentSpawnTime;

    private void Update()
    {
        _currentSpawnTime += Time.deltaTime;

        if (_currentSpawnTime >= SpawnDelay)
        {
            var arrow = Instantiate(_arrow, _spawnPoint.position, transform.rotation);
            _currentSpawnTime = 0;
            arrow.transform.parent = transform;
        }
    }
}
