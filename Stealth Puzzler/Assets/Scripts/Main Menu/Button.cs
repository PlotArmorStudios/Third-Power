using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Button : MonoBehaviour
{
    [SerializeField] private TMP_Text _buttonText;
    [SerializeField] private float _onHoverTextSize;
    [SerializeField] private float _textSizeDefault;
    private float _transitionTime = .5f;

    public void OnPointerEnter()
    {
        LeanTween.cancel(gameObject);
        LeanTween.value(gameObject, a => _buttonText.fontSize = a, _textSizeDefault, _onHoverTextSize, _transitionTime);
    }
    public void OnPointerExit()
    {
        LeanTween.cancel(gameObject);
        LeanTween.value(gameObject, a => _buttonText.fontSize = a, _onHoverTextSize, _textSizeDefault, _transitionTime);
    }

    public void OnClicked()
    {
        LeanTween.value(gameObject, a => _buttonText.fontSize = a, _onHoverTextSize, _textSizeDefault, _transitionTime);
    }
}
