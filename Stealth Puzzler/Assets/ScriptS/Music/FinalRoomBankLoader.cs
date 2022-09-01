using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalRoomBankLoader : MonoBehaviour
{
    public AK.Wwise.Event Play_Cymbals;

    void Awake()
    {
        AkBankManager.LoadBank("Final_Level_Music_Bank", false, false);
    }

    private void Start()
    {
        Play_Cymbals.Post(gameObject);
    }
}
