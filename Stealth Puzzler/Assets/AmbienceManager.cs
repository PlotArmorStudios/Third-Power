using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmbienceManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("Play_Dungeon_1", gameObject);
       // DontDestroyOnLoad(this.gameObject);
    }

    
}
