using System;
using System.Collections;
using System.Collections.Generic;
using Helpers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;


/// <summary>
/// Should always be paired with a SceneLoader game object.
/// </summary>

[DefaultExecutionOrder(100)]
public class GameManager : MonoBehaviour
{
    public ControllerManager ControllerManager;

    public static GameManager Instance;

    public int CurrentLevel { get; private set; }
    public float[] CurrentPosition = new float[3];
    public bool LoadedFromSave { get; set; }
    public Dictionary<string, bool> ObstacleBooleans { get; set; }

    [HideInInspector] public Vector3 PlayerSpawnLocation;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        ObstacleBooleans = new Dictionary<string, bool>();
    }

    public void SaveGame()
    {
        var controller = FindObjectOfType<Controller>();
        var controllerPosition = controller.transform.position;
        CurrentPosition = new float[] {controllerPosition.x, controllerPosition.y, controllerPosition.z};
        Debug.Log("Saved position. " + CurrentPosition[0] + ", "+ CurrentPosition[1] + ", "+ CurrentPosition[1]);
        SaveSystem.SaveGame(this);
    }

    public void LoadGame()
    {
        GameData data = SaveSystem.LoadGame();

        LoadedFromSave = true;
        CurrentLevel = data.CurrentLevel;
        CurrentPosition = data.CurrentPosition;
        ObstacleBooleans = data.TrapBooleans;

        PlayerSpawnLocation = new Vector3();

        PlayerSpawnLocation.x = CurrentPosition[0];
        PlayerSpawnLocation.y = CurrentPosition[1];
        PlayerSpawnLocation.z = CurrentPosition[2];

        WwiseMusic.MusicInstance.PlayMusic();

        Debug.Log("Should Load Game. Level to load: " + CurrentLevel);
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
        var controllerManager = Instantiate(ControllerManager, defaultSpawn.position, defaultSpawn.rotation);
        var camRig = FindObjectOfType<CamRig>();
        controllerManager.InitializeControllers(Camera.main);
        camRig.GetComponentInChildren<FocalPointManager>().InitializeFocalPoints(controllerManager);
        yield return new WaitForSeconds(.2f);
    }

    private IEnumerator SequentialInstantiate()
    {
        //TODO: Verify that this does not cause an issue with player rotation on load.
        Debug.Log("Spawned from Save. Spawn location: " + PlayerSpawnLocation);
        var camRig = FindObjectOfType<CamRig>();
        Debug.Log("Cam rig found: " + camRig);
        var controllerManager = Instantiate(ControllerManager, PlayerSpawnLocation, Quaternion.identity);
        controllerManager.InitializeControllers(Camera.main);
        camRig.GetComponentInChildren<FocalPointManager>().InitializeFocalPoints(controllerManager);
        yield return new WaitForSeconds(.2f);
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

    /// <summary>
    /// Loads player to the default position of that loaded scene.
    /// </summary>
    /// <param name="scene"></param>
    /// <param name="mode"></param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("OnSceneLoaded: " + scene.name);
        Instance.CurrentLevel = FindObjectOfType<LevelData>().LevelIndex;
        WwiseMusic.MusicInstance.PlayMusic();
    }

    public void AddObstacleBoolean(string obstacleID, bool isOpen)
    {
        Instance.ObstacleBooleans.Add(obstacleID, isOpen);
        Debug.Log("Saved " + obstacleID);
    }
}