using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Helpers;

public class MainMenu : MonoBehaviour
{
    public void OnPlayClicked()
    {
        SceneLoader.Instance.LoadScene("Level 1");
    }
}
