using System.Collections;
using Helpers;
using UnityEngine;

public class UIEndLogo : MonoBehaviour
{
    [SerializeField] private RectTransform _UICanvas;
    [SerializeField] private float _logoShowTime = 7f;
    [SerializeField] private string _sceneToLoadAfterLogo;
    [SerializeField] private float _timeToReturnToMenuAfterLogo = 3f;

    private IEnumerator Start()
    {
        ShowUI();
        yield return new WaitForSeconds(_logoShowTime);
        HideUI();
        yield return new WaitForSeconds(_timeToReturnToMenuAfterLogo);
        SceneLoader.Instance.LoadScene(_sceneToLoadAfterLogo);
    }

    private void ShowUI()
    {
        LeanTween.alpha(_UICanvas, 1f, 0.5f).setFrom(0);
    }
    private void HideUI()
    {
        LeanTween.alpha(_UICanvas, 0, 0.5f);
    }
}