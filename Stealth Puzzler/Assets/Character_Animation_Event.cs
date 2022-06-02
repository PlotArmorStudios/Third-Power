using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character_Animation_Event : MonoBehaviour
{


    public bool Debug_Enabled = false;

    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.RegisterGameObj(gameObject);
    }

    void fs_Left_Foot()
    {
        Debug.Log("Left Foot Triggered");
        AkSoundEngine.PostEvent("Play_Footsteps", gameObject);
    }

    void fs_Right_Foot()
    {
        Debug.Log("Right Foot Triggered");
        AkSoundEngine.PostEvent("Play_Footsteps", gameObject);
    }
}
