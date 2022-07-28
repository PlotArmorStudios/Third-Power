using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseSfxManager : MonoBehaviour
{
    public AK.Wwise.Event Play_UI_StartGame;
    public AK.Wwise.Event Play_UI_MainMenu;
    public AK.Wwise.Event Play_UI_NormalClick;
    public AK.Wwise.Event Play_UI_Hover;
    public AK.Wwise.Event StopMainMenu_Ambience;

   public void PlayUIStartGameSound()
    {
        Play_UI_StartGame.Post(gameObject);
    }

   public void PlayUIMainMenuSound()
    {
        Play_UI_MainMenu.Post(gameObject);
    }

   public void PlayUINormalClickSound()
    {
        Play_UI_NormalClick.Post(gameObject);
    }
    public void PlayUIHoverSound()
    {
        Play_UI_Hover.Post(gameObject);
    }

    public void StopMainMenuAmbience()
    {
        StopMainMenu_Ambience.Post(gameObject);
    }

}
