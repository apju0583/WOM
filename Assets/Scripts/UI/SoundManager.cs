using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource[] audios;
    public AudioMixer mixer;
    public Slider BGMSlider;
    public Slider SFXSlider;

    void Awake() 
    {
        if (instance == null) 
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

        else 
        {
            if (instance != this) 
            {
                Destroy(this.gameObject);
            }
        }

        audios = GetComponents<AudioSource>();
    }

    void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDisable() 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        switch (scene.name) 
        {
            case "Start Scene":
                for (int i = 0; i < audios.Length; i++) {
                    if (i != 0) {
                        audios[i].Stop();
                    }
                }
                audios[0].Play();
                break;

            case "Village":
                for (int i = 0; i < audios.Length; i++) {
                    if (i != 1) {
                        audios[i].Stop();
                    }
                }
                audios[1].Play();
                break;

            default:
                break;
        }
    }

    public void BGMControl() 
    {
        float sound = BGMSlider.value;

        if (sound == -40f) {
            mixer.SetFloat("BGM", -80);
        }

        else {
            mixer.SetFloat("BGM", sound);
        }
    }

    public void SFXControl() 
    {
        float sound = SFXSlider.value;

        if (sound == -40f) {
            mixer.SetFloat("SFX", -80);
        }

        else {
            mixer.SetFloat("SFX", sound);
        }
    }
}