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
    }

    public void PlayMusic()
    {
        Play_Music.Post(gameObject);
        IsPlaying = true;
    }


    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.N))
    //        Play_Music.Post(gameObject);
    //}
}
