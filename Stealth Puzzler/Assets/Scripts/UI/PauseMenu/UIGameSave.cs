using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGameSave : MonoBehaviour
{
    [SerializeField] private Image _gameSavedImage;
    [SerializeField] private float _displayTime = 1f;
    [SerializeField] private float _fadeSpeed;

    public void TriggerSaveUI()
    {
        StopCoroutine(SetFade());
        StartCoroutine(SetFade());
    }

    private IEnumerator SetFade()
    {
        float alpha = 1;
        _gameSavedImage.color = new Color(1, 1, 1, 1);

        yield return new WaitForSecondsRealtime(_displayTime);
        
        while (alpha > .01f)
        {
            alpha -= Time.unscaledDeltaTime * _fadeSpeed;
            Debug.Log("Fade text");
            _gameSavedImage.color = new Color(1, 1, 1, alpha);
            yield return null;
        }
    }
}