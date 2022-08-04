using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageGraphics : MonoBehaviour
{
    private float _tweenTime = .25f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void FadeImageIn(CanvasGroup _canvasGroup)
    {
        LeanTween.alphaCanvas(_canvasGroup, 1, _tweenTime).setIgnoreTimeScale(true);
    }
    public void FadeImageOut(CanvasGroup _canvasGroup)
    {
        LeanTween.alphaCanvas(_canvasGroup, 0, _tweenTime).setIgnoreTimeScale(true);
    }
}
