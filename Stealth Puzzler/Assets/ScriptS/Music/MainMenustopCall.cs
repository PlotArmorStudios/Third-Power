using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenustopCall : MonoBehaviour
{

    public AK.Wwise.Event Stop_Music;

    void Start()
    {
        Stop_Music.Post(gameObject);
    }
}
