using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControle : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider musicaSlider;
    public Slider sfxSlider;
    public Slider uiSlider;

    void Start()
    {
        musicaSlider.onValueChanged.AddListener(SetMusicaVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        uiSlider.onValueChanged.AddListener(SetUIVolume);
    }

    public void SetMusicaVolume(float valor)
    {
        mixer.SetFloat("MUSICA_VOL", Mathf.Log10(valor) * 20);
    }

    public void SetSFXVolume(float valor)
    {
        mixer.SetFloat("SFX_VOL", Mathf.Log10(valor) * 20);
    }

    public void SetUIVolume(float valor)
    {
        mixer.SetFloat("UI_VOL", Mathf.Log10(valor) * 20);
    }
}
