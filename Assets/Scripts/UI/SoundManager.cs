using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    public AudioSource[] audios; // 시작, 마을, 상점, 하부, 중부, 집가는길이랑 집, 집안, 버섯굴, 호수가는길이랑 호수, 정상 순으로 BGM / 공격, 피격(플레이어, 몬스터 동일), 몬스터 사망, 아이템 매매, 아이템 사용, 채집완료 순으로 SFX
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

            case "Store_Armor":
            case "Store_Food":
            case "Store_Hunter":
            case "Store_Weapon":
                for (int i = 0; i < audios.Length; i++) {
                    if (i != 2) {
                        audios[i].Stop();
                    }
                }
                audios[2].Play();
                break;

            case "Mountain_Bot1":
            case "Mountain_Bot2":
                for (int i = 0; i < audios.Length; i++) {
                    if (i != 3) {
                        audios[i].Stop();
                    }
                }
                audios[3].Play();
                break;

            case "Mountain_Mid1":
            case "Mountain_Mid2":
                for (int i = 0; i < audios.Length; i++) {
                    if (i != 4) {
                        audios[i].Stop();
                    }
                }
                audios[4].Play();
                break;

            case "Mountain_BotHouse":
            case "Mountain_Way_To_House":
                for (int i = 0; i < audios.Length; i++) {
                    if (i != 5) {
                        audios[i].Stop();
                    }
                }
                audios[5].Play();
                break;

            case "Mountain_Player_House":
                for (int i = 0; i < audios.Length; i++) {
                    if (i != 6) {
                        audios[i].Stop();
                    }
                }
                audios[6].Play();
                break;

            case "Mountain_Mushroom_Cave":
                for (int i = 0; i < audios.Length; i++) {
                    if (i != 7) {
                        audios[i].Stop();
                    }
                }
                audios[7].Play();
                break;

            case "Mountain_MidLake":
            case "Mountain_Way_To_Lake":
                for (int i = 0; i < audios.Length; i++) {
                    if (i != 8) {
                        audios[i].Stop();
                    }
                }
                audios[8].Play();
                break;

            case "Mountain_Top":
                for (int i = 0; i < audios.Length; i++) {
                    if (i != 9) {
                        audios[i].Stop();
                    }
                }
                audios[9].Play();
                break;

            default:
                for (int i = 0; i < audios.Length; i++) {
                    audios[i].Stop();
                }
                break;
        }
    }

    public void SFXPlay(int index) {
        for (int i = 10; i < audios.Length; i++) {
            if (i != index) {
                audios[index].Stop();
            }
        }
        audios[index].Play();
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