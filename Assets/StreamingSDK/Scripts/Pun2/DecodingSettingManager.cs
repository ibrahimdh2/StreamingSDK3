using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMETP;
using TMPro;
using UnityEngine.UI;

public class DecodingSettingManager : MonoBehaviour
{
    public GameViewDecoder decoder;
    public AudioDecoder audioDecoder;   
    public Slider volumeSlider;
    public Toggle fastDecodeToggle;
    
  
    public void ChangeVolume()
    {
        audioDecoder.Volume = volumeSlider.value;   
    }
    public void ToggleFastDecoding()
    {
        decoder.FastMode = fastDecodeToggle.isOn;
    }
}
