using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ImageTween : MonoBehaviour
{
    [SerializeField] private float _onHoverSize = 1.2f;
    [SerializeField] private float _defaultSize = 1;
    private float _transitionTime = .5f;

    public void OnPointerEnter()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, new Vector3(_onHoverSize, _onHoverSize), _transitionTime).setIgnoreTimeScale(true);
    }
    public void OnPointerExit()
    {
        LeanTween.cancel(gameObject);
        LeanTween.scale(gameObject, new Vector3(_defaultSize, _defaultSize), _transitionTime).setIgnoreTimeScale(true);
    }
}
