using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private void OnEnable()
    {
        AkSoundEngine.PostEvent("Play_Arrow_In_Air", gameObject);
    }

    private void OnDisable()
    {
        AkSoundEngine.PostEvent("Stop_Arrow_In_Air", gameObject);
    }

}