using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OnCreditsEnd : MonoBehaviour
{
    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _creditsMask;
    public void OnCreditsEnding()
    {
        _creditsMask.SetActive(false);
        _mainMenu.SetActive(true);

    }
}
