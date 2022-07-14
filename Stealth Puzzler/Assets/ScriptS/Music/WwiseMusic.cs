using UnityEngine;
using UnityEngine.SceneManagement;

public class WwiseMusic : MonoBehaviour
{
    public static WwiseMusic MusicInstance;

    private WwiseMusicLevelNumber _wwiseLevelNumber;

    public AK.Wwise.Event Play_Early_Music;
    public AK.Wwise.Event Play_Later_Music;

    [HideInInspector]
    public bool IsPlaying  = false;
    [HideInInspector]
    public bool BankIsLoaded = false;

    //private int _levelNumber;

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

        _wwiseLevelNumber = FindObjectOfType<WwiseMusicLevelNumber>();

        //This should just be for in engine music. Starting directly in a scene
        if (SceneManager.GetActiveScene().name != "Main Menu")
            if (IsPlaying == false)
                if (_wwiseLevelNumber.levelNumber < 7)
                {
                    Play_Early_Music.Post(gameObject);
                    IsPlaying = true;
                }
                else if (_wwiseLevelNumber.levelNumber >= 7)
                {
                    Play_Later_Music.Post(gameObject);
                    IsPlaying = true;
                }
        //_levelNumber = FindObjectOfType<LevelData>().LevelIndex;
    }

    public void PlayMusic()
    {
        //Called from Play in MainMenu and in GameManager's OnSceneLoaded
        if (GameManager.Instance.CurrentLevel < 7)
        {
            Play_Early_Music.Post(gameObject);
            IsPlaying = true;
        }
        else if(GameManager.Instance.CurrentLevel >= 7)
        {
            Play_Later_Music.Post(gameObject);
            IsPlaying = true;
        }
    }


    //private void OnEnable()
    //{
    //    SceneManager.sceneLoaded += OnSceneLoaded;
    //}

    //private void OnDisable()
    //{
    //    SceneManager.sceneLoaded -= OnSceneLoaded;
    //}

    //private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    //{
    //    if (SceneManager.GetActiveScene().name != "Main Menu")
    //        AkSoundEngine.SetRTPCValue("LevelNumber", _levelNumber);
    //    Debug.Log("For Mark: Level is Number " + _levelNumber);
    //}
}
