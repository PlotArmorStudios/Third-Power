using UnityEngine;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    public Slider thisSlider;
    private float sliderVolume;
    public string Bus;


    void Start()
    {
        float VolumeRTPC;
        int reftype = 1;
        AkSoundEngine.GetRTPCValue(Bus, null, 0, out VolumeRTPC, ref reftype);
        //Debug.Log(Bus + " : " + VolumeRTPC);

        thisSlider.value = VolumeRTPC;
        sliderVolume = thisSlider.value;
    }

    public void OnVolumeChange()
    {
            sliderVolume = thisSlider.value;
            AkSoundEngine.SetRTPCValue(Bus, sliderVolume);
    }

    public void Mute()
    {
            thisSlider.value = 0;
            AkSoundEngine.SetRTPCValue(Bus, 0);
    }
}
