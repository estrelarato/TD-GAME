using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider sliderMusica;
    public Slider sliderSFX;
    public Slider sliderUI;

    private void Start()
    {
        sliderMusica.onValueChanged.AddListener(SetVolumeMusica);
        sliderSFX.onValueChanged.AddListener(SetVolumeSFX);
        sliderUI.onValueChanged.AddListener(SetVolumeUI);
    }

    public void SetVolumeMusica(float volume)
    {
        audioMixer.SetFloat("MUSICA Volume", volume);
    }

    public void SetVolumeSFX(float volume)
    {
        audioMixer.SetFloat("SFX Volume", volume);
    }

    public void SetVolumeUI(float volume)
    {
        audioMixer.SetFloat("UI Volume", volume);
    }
}
