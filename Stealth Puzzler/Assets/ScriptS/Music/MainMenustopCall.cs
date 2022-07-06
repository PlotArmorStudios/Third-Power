using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenustopCall : MonoBehaviour
{

    public AK.Wwise.Event Stop_Music;

    private WwiseMusic _wwiseMusic;

    void Start()
    {
        _wwiseMusic = GetComponent<WwiseMusic>();

        if (_wwiseMusic.IsPlaying == true)
            Stop_Music.Post(gameObject);
    }
}
