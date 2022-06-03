using System.Collections;
using UnityEngine;

public class ToggleArrowTrap : MonoBehaviour
{
    [SerializeField] private GameObject _arrow;
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float _shootDelay;
    
    public float SpawnDelay;
    
    public bool IsActive;
    
    private float _currentSpawnTime;
    private WaitForSeconds _wait;

    private void Awake()
    {
        _wait = new WaitForSeconds(_shootDelay);
    }

    private void OnEnable()
    {
        StartCoroutine(DelayShoot());
    }

    private IEnumerator DelayShoot()
    {
        yield return _wait;
        ShootArrow();
    }

    private void OnDisable()
    {
        StopCoroutine(DelayShoot());
    }
    
    private void ShootArrow()
    {
        var arrow = Instantiate(_arrow, _spawnPoint.position, transform.rotation);
        _currentSpawnTime = 0;
    }

}