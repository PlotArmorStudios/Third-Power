using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseMusic : MonoBehaviour
{
    private static WwiseMusic _instance;

    public AK.Wwise.Event Play_Music;

    private void Awake()
    {
        if(_instance == null) {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else {
            Destroy(this.gameObject);
        }
    }

    public void PlayMusic()
    {
        Play_Music.Post(gameObject);
    }

    void Update()
    {
        
    }
}
