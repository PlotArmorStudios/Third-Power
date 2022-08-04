using UnityEngine;

public class WwiseVoiceOver : MonoBehaviour
{
    public AK.Wwise.Event VO1;

    private bool _firstTime = true;

    void OnTriggerEnter()
    {
        if (_firstTime)
        {
            VO1.Post(gameObject);
            _firstTime = false;
        }
    }
}
