using UnityEngine;
using UnityEngine.SceneManagement;


public class FinalRoomBankLoader : MonoBehaviour
{
    public static FinalRoomBankLoader Instance;

    public AK.Wwise.Event Play_Cymbals;

    void Awake()
    {
        AkBankManager.LoadBank("Final_Level_Music_Bank", false, true);

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        } else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        Play_Cymbals.Post(gameObject);
        AkSoundEngine.SetRTPCValue("LevelNumber", 1);
    }

    public void MainMenuCheck()
    {
        if (SceneManager.GetActiveScene().name == "Main Menu")
        {
            Destroy(this.gameObject);
            Debug.Log("Final Level Bank Object Destroyed");
        }
    }

    private void OnDestroy()
    {
        AkBankManager.UnloadBank("Final_Level_Music_Bank");
        Debug.Log("Final Level Bank Unloaded");
    }
}
