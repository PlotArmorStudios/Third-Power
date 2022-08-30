using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class VolumeSliders : MonoBehaviour
{
    public Slider thisSlider;
    private float sliderVolume;
    public string Bus;

    void Start() => LoadSlider();

    
    private void SetDefaultVolume()
    {
        var VolumeRTPC = GetRTPCVolume(Bus);
        thisSlider.value = VolumeRTPC;
    }

    public float GetRTPCVolume(string bus)
    {
        float VolumeRTPC;
        int reftype = 1;
        AkSoundEngine.GetRTPCValue(bus, null, 0, out VolumeRTPC, ref reftype);
        return VolumeRTPC;
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

    private void LoadSlider()
    {
        var data = SaveSystem.LoadGame();
        
        if (Bus == "MasterVolume") sliderVolume = GameManager.Instance.MasterVolume;
        if (Bus == "MusicVolume") sliderVolume = GameManager.Instance.MusicVolume;
        if (Bus == "SFXVolume") sliderVolume = GameManager.Instance.SFXVolume;

        if(sliderVolume == 0) SetDefaultVolume();
            
        thisSlider.value = sliderVolume;
        
        AkSoundEngine.SetRTPCValue(Bus, sliderVolume);
    }
}