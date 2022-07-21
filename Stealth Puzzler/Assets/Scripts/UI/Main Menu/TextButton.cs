using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextButton : MonoBehaviour
{
    [SerializeField] private TMP_Text _buttonText;
    [SerializeField] private float _onHoverTextSize;
    [SerializeField] private float _textSizeDefault;
    private float _transitionTime = .5f;

    public void OnPointerEnter()
    {
        LeanTween.cancel(gameObject);
        LeanTween.value(gameObject, a => _buttonText.fontSize = a, _textSizeDefault, _onHoverTextSize, _transitionTime).setIgnoreTimeScale(true);
    }
    public void OnPointerExit()
    {
        LeanTween.cancel(gameObject);
        LeanTween.value(gameObject, a => _buttonText.fontSize = a, _onHoverTextSize, _textSizeDefault, _transitionTime).setIgnoreTimeScale(true);
    }
}
