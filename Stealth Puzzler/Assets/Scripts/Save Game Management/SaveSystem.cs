using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.Rendering.Universal;

public static class SaveSystem
{
    public static void SaveGame(GameManager gameManager)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/game.txt";

        FileStream stream = new FileStream(path, FileMode.Create);

        GameData data = new GameData(gameManager);

        Debug.Log("Game saved. Path: " + path);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static GameData LoadGame()
    {
        string path = Application.persistentDataPath + "/game.txt";

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
}

// A class must be serializable to be converted to and from JSON by JsonUtility.
[System.Serializable]
public class GameData
{
    public int CurrentLevel;
    public float[] CurrentPosition;

    public GameData(GameManager gameManager)
    {
        CurrentLevel = gameManager.CurrentLevel;
        CurrentPosition = gameManager.CurrentPosition;
    }
}