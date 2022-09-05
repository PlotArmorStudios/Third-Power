using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Rendering.Universal;

public static class SaveSystem
{
    public static event Action OnDeleteSave;
    public static void SaveGame(GameManager gameManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/ThirdPowerD.txt";

        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(gameManager);

        Debug.Log("Game saved. Path: " + path);
        
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadGame()
    {
        string path = Application.persistentDataPath + "/ThirdPowerD.txt";

        if (File.Exists((path)))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            GameData data = formatter.Deserialize(stream) as GameData;
            Debug.Log("Game Loaded. Path: " + path);
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("Save file not found in " + path);
            return null;
        }
    }

    public static void DeleteSave()
    {
        string path = Application.persistentDataPath + "/ThirdPowerD.txt";
            
        if (File.Exists((path)))
        {
            Debug.Log("Game save deleted. Path: " + path);
            File.Delete((path));
        }
        
        OnDeleteSave?.Invoke();
    }

}

// A class must be serializable to be converted to and from JSON by JsonUtility.
[System.Serializable]
public class GameData
{
    public int CurrentLevel;
    public float[] CurrentPosition;
    public float[] CurrentRotation;

    public float MasterVolume;
    public float MusicVolume;
    public float SFXVolume;

    public bool CursorLockToggle;
    
    public Dictionary<string, bool> TrapBooleans;

    public GameData(GameManager gameManager)
    {
        CurrentLevel = gameManager.CurrentLevel;
        CurrentPosition = gameManager.CurrentPosition;
        CurrentRotation = gameManager.CurrentRotation;
        TrapBooleans = gameManager.ObstacleBooleans;

        MasterVolume = gameManager.MasterVolume;
        MusicVolume = gameManager.MusicVolume;
        SFXVolume = gameManager.SFXVolume;

        CursorLockToggle = gameManager.CursorLockToggle;
        
        Debug.Log("Saved position in file: " + CurrentPosition[0] + ", "+ CurrentPosition[1] + ", "+ CurrentPosition[2]);
    }
}