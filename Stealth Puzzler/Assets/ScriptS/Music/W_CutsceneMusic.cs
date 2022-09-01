using UnityEngine;

public class W_CutsceneMusic : MonoBehaviour
{
    public AK.Wwise.Event Play_Cutscene_Music;
    private bool _canPlay = true;

    void OnTriggerEnter()
    {
        if (_canPlay)
        {
            Play_Cutscene_Music.Post(gameObject);
            _canPlay = false;
        }
    }
}
