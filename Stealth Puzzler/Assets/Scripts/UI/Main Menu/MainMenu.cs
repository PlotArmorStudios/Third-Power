using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

public class MainMenu : MonoBehaviour
{
    public void OnPlayClicked()
    {
        GameManager.Instance.DeleteSave();
        SceneLoader.Instance.LoadScene("Level 1");
    }

    public void OnLoadClicked()
    {
        GameManager.Instance.LoadGame();
    }
}
