using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    [SerializeField] private Transform _defaultSpawnPoint;

    private void Start()
    {
        _defaultSpawnPoint = transform;
        GameManager.Instance.SpawnPlayer(_defaultSpawnPoint);
    }

    public void SpawnDefault()
    {
//        Instantiate(Game)
    }
}