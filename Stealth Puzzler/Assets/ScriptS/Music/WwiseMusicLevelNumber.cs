using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseMusicLevelNumber : MonoBehaviour
{

    public int levelNumber;

    void Start()
    {
        AkSoundEngine.SetRTPCValue("LevelNumber", levelNumber);
    }
}
