using UnityEngine;

public class CutsceneSounds : MonoBehaviour
{
    public void PlayCutsceneSound()
    {
        AkSoundEngine.PostEvent("Play_SFX_Cutscene_Arrow_Demonstartion", gameObject);

    }
}