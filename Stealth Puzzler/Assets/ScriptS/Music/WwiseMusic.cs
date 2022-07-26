using UnityEngine;
using UnityEngine.SceneManagement;

public class WwiseMusic : MonoBehaviour
{
    public static WwiseMusic MusicInstance;

    public AK.Wwise.Event Play_Early_Music;
    public AK.Wwise.Event Play_Later_Music;
    public AK.Wwise.Event Start_With_Later_Music;
    public AK.Wwise.Event Stop_Music;

    [HideInInspector]
    public bool IsPlaying  = false;
    [HideInInspector]
    public bool BankIsLoaded = false;

    private void Awake()
    {
        if(MusicInstance == null)
        {
            MusicInstance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }

        AkBankManager.LoadBank("Music_Bank", false, false);
        BankIsLoaded = true;
    }

    void Start()
    {
        if (BankIsLoaded == false)
            AkBankManager.LoadBank("Music_Bank", false, false);
    }

    //Called from GameManager's OnSceneLoaded
    public void PlayMusic()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
            if (IsPlaying == true)
            {
                Stop_Music.Post(gameObject);
                IsPlaying = false;
            }

        if (SceneManager.GetActiveScene().name != "Main Menu")
            if (IsPlaying == false)
                if (GameManager.Instance.CurrentLevel < 9)
                {
                    Play_Early_Music.Post(gameObject);
                    IsPlaying = true;
                }
                else if(GameManager.Instance.CurrentLevel >= 9)
                {
                    Start_With_Later_Music.Post(gameObject);
                    IsPlaying = true;
                }
            if (IsPlaying == true)
                {
                    if (GameManager.Instance.CurrentLevel == 9 || GameManager.Instance.CurrentLevel > 9)
                    {
                        Play_Later_Music.Post(gameObject);
                        //Debug.Log("Music transitioned.");
                    }
                    //else
                        //Debug.Log("Music didn't transition");
                }
    }
}
