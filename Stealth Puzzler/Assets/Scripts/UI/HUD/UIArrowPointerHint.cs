using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIArrowPointerHint : MonoBehaviour
{
    [SerializeField] private Transform _position1;
    [SerializeField] private Transform _position2;
    [SerializeField] private float _halfCycleTime = 0.5f;
    // Start is called before the first frame update


    public void PlayAnimation() 
    {
        LeanTween.cancel(this.gameObject);
        LeanTween.alpha(this.gameObject, 1, 0.75f);
        LeanTween.move(this.gameObject, _position2.position, _halfCycleTime).setFrom(_position1.position).setLoopPingPong().setRepeat(-1);
    }

    public void StopAnimation()
    {
        LeanTween.cancel(this.gameObject);
        LeanTween.alpha(this.gameObject, 0, 0.75f);
    }
}
