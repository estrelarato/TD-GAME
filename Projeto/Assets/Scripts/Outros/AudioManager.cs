using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;


    public AudioMixer audioMixer;


    public AudioSource musicSource;


    public AudioClip musicaMenu;
    public AudioClip musicaJogo;
    public AudioClip musicaBoss;

    private void Awake()
    {

        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {

        float musicVol = PlayerPrefs.GetFloat("MusicVolume", 1f);
        float sfxVol   = PlayerPrefs.GetFloat("SFXVolume", 1f);
        float uiVol    = PlayerPrefs.GetFloat("UIVolume", 1f);

        UpdateMusicVolume(musicVol);
        UpdateSFXVolume(sfxVol);
        UpdateUIVolume(uiVol);


        TrocarMusicaDaCena(SceneManager.GetActiveScene().name);
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        TrocarMusicaDaCena(scene.name);
    }


    public void TrocarMusicaDaCena(string sceneName)
    {
        if (sceneName.Contains("Menu"))
        {
            TrocarMusica(musicaMenu);
        }
        else if (sceneName.Contains("Boss"))
        {
            TrocarMusica(musicaBoss);
        }
        else
        {
            TrocarMusica(musicaJogo);
        }
    }


    public void TrocarMusica(AudioClip novaMusica)
    {
        if (musicSource.clip == novaMusica && musicSource.isPlaying) return;

        musicSource.clip = novaMusica;
        musicSource.loop = true;
        musicSource.Play();
    }


    public void UpdateMusicVolume(float value)
    {
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(Mathf.Clamp(value, 0.001f, 1f)) * 20);
        PlayerPrefs.SetFloat("MusicVolume", value);
    }

    public void UpdateSFXVolume(float value)
    {
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(Mathf.Clamp(value, 0.001f, 1f)) * 20);
        PlayerPrefs.SetFloat("SFXVolume", value);
    }

    public void UpdateUIVolume(float value)
    {
        audioMixer.SetFloat("UIVolume", Mathf.Log10(Mathf.Clamp(value, 0.001f, 1f)) * 20);
        PlayerPrefs.SetFloat("UIVolume", value);
    }
}
