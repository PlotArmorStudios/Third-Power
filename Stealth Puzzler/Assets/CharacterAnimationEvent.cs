//#define DebugLog
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationEvent : MonoBehaviour
{
    void Start()
    {
        AkSoundEngine.RegisterGameObj(gameObject);
    }

    void fs_Left_Foot()
    {
#if DebugLog
        Debug.Log("Left Foot Triggered");
#endif
        AkSoundEngine.PostEvent("Play_Footsteps", gameObject);
    }

    void fs_Right_Foot()
    {
#if DebugLog
        Debug.Log("Right Foot Triggered");
#endif
        AkSoundEngine.PostEvent("Play_Footsteps", gameObject);
    }
}
