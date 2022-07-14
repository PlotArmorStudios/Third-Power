using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkBankLoader : MonoBehaviour
{
    //public AK.Wwise.Event Play_Music;

    void Start()
    {
        AkBankManager.LoadBank("Music_Bank", false, false);
    }

    //void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.N))
    //        Play_Music.Post(gameObject);
    //}
}
