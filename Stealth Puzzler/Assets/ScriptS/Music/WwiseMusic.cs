using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WwiseMusic : MonoBehaviour
{
    private static WwiseMusic _instance;

    public AK.Wwise.Event Play_Music;

    [HideInInspector]
    public bool IsPlaying  = false;
    [HideInInspector]
    public bool BankIsLoaded = false;

    //private int _levelNumber;

    private void Awake()
    {
        if(_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        AkBankManager.LoadBank("Music_Bank", false, false);
        BankIsLoaded = true;
    }

    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Main Menu")
            if (IsPlaying == false)
                Play_Music.Post(gameObject); 
                IsPlaying = true;
        if (BankIsLoaded == false)
            AkBankManager.LoadBank("Music_Bank", false, false);

        //_levelNumber = FindObjectOfType<LevelData>().LevelIndex;
    }

    //private void OnEnable()
    //{
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    //private void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    if (SceneManager.GetActiveScene().name != "Main Menu")
    //        AkSoundEngine.SetRTPCValue("LevelNumber", _levelNumber);
    //    Debug.Log("For Mark: Level is Number " + _levelNumber);
    //}

    public void PlayMusic()
    {
        Play_Music.Post(gameObject);
        IsPlaying = true;
    }
}
