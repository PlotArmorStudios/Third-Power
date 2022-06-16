using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;


/// <summary>
/// Should always be paired with a SceneLoader game object.
/// </summary>
public class GameManager : MonoBehaviour
{
    public ControllerManager ControllerManager;
    [SerializeField] private GameObject _camRig;

    public static GameManager Instance;

    public int CurrentLevel { get; private set; }
    public float[] CurrentPosition { get; set; }
    public bool LoadedFromSave { get; set; }

    [HideInInspector]
    public Vector3 PlayerSpawnLocation;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    public void SaveGame()
    {
        SaveSystem.SaveGame(this);
    }

    public void LoadGame()
    {
        GameData data = SaveSystem.LoadGame();

        LoadedFromSave = true;
        CurrentLevel = data.CurrentLevel;
        CurrentPosition = data.CurrentPosition;

        PlayerSpawnLocation.x = CurrentPosition[0];
        PlayerSpawnLocation.y = CurrentPosition[1];
        PlayerSpawnLocation.z = CurrentPosition[2];

        SceneLoader.Instance.LoadScene("Level " + CurrentLevel);
    }

    public void SpawnPlayer(Transform defaultSpawn)
    {
        if (Instance.LoadedFromSave)
            StartCoroutine(SequentialInstantiate());
        else
            StartCoroutine(SequentialInstantiate(defaultSpawn));
    }

    private IEnumerator SequentialInstantiate(Transform defaultSpawn)
    {
        Instantiate(ControllerManager, defaultSpawn.position, Quaternion.identity);
        yield return new WaitForSeconds(.2f);
        Instantiate(_camRig, transform.position, Quaternion.identity);
    }

    private IEnumerator SequentialInstantiate()
    {
        Instantiate(ControllerManager, PlayerSpawnLocation, Quaternion.identity);
        yield return new WaitForSeconds(.2f);
        Instantiate(_camRig, transform.position, Quaternion.identity);
        LoadedFromSave = false;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    //Check for scene load issues
    /// <summary>
    /// Loads player to the default position of that loaded scene.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);

        if (!Instance.LoadedFromSave)
        {
            //TODO: Load player at default spawn area if not loading game
            //FindObjectOfType<PlayerSpawner>().SpawnDefault();
        }
    }
}