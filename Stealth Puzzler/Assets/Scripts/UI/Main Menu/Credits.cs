using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Credits : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _creditsPanel;
    // Start is called before the first frame update

    public void OnCreditsClicked()
    {
        _mainMenu.SetActive(false);
        _creditsPanel.SetActive(true);
    }
}
