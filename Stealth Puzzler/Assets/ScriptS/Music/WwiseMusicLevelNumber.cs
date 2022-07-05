using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseMusicLevelNumber : MonoBehaviour
{
    [SerializeField]
    private int _levelNumber;

    void Start()
    {
        AkSoundEngine.SetRTPCValue("LevelNumber", _levelNumber);
    }
}
