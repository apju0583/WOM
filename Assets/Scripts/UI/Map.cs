using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Map : MonoBehaviour
{
    [SerializeField] Animator[] areaAnims;

    public void Init() 
    {
        areaAnims = GetComponentsInChildren<Animator>();
    }

    void OnEnable() 
    {
        Scene scene = SceneManager.GetActiveScene();

        switch (scene.name) {
            case "Town":
            case "Village":
            case "Store_Armor":
            case "Store_Food":
            case "Store_Hunter":
            case "Store_Weapon":
                for (int i = 0; i < areaAnims.Length; i++) {
                    if (i == 0) {
                        areaAnims[i].SetBool("isLocate", true);
                    }
                    else {
                        areaAnims[i].SetBool("isLocate", false);
                    }
                }
                break;

            case "Lower Mountain 1":
            case "Mountain_Bot1":
                for (int i = 0; i < areaAnims.Length; i++) {
                    if (i == 1) {
                        areaAnims[i].SetBool("isLocate", true);
                    }
                    else {
                        areaAnims[i].SetBool("isLocate", false);
                    }
                }
                break;

            case "Way Home":
            case "Mountain_Way_To_House":
                for (int i = 0; i < areaAnims.Length; i++) {
                    if (i == 2) {
                        areaAnims[i].SetBool("isLocate", true);
                    }
                    else {
                        areaAnims[i].SetBool("isLocate", false);
                    }
                }
                break;

            case "Home":
            case "Mountain_BotHouse":
            case "Mountain_Player_House":
                for (int i = 0; i < areaAnims.Length; i++) {
                    if (i == 3) {
                        areaAnims[i].SetBool("isLocate", true);
                    }
                    else {
                        areaAnims[i].SetBool("isLocate", false);
                    }
                }
                break;

            case "Lower Mountain 2":
            case "Mountain_Bot2":
                for (int i = 0; i < areaAnims.Length; i++) {
                    if (i == 4) {
                        areaAnims[i].SetBool("isLocate", true);
                    }
                    else {
                        areaAnims[i].SetBool("isLocate", false);
                    }
                }
                break;

            case "Center Mountain 1":
            case "Mountain_Mid1":
                for (int i = 0; i < areaAnims.Length; i++) {
                    if (i == 5) {
                        areaAnims[i].SetBool("isLocate", true);
                    }
                    else {
                        areaAnims[i].SetBool("isLocate", false);
                    }
                }
                break;

            case "Way Lake":
            case "Mountain_Way_To_Lake":
                for (int i = 0; i < areaAnims.Length; i++) {
                    if (i == 6) {
                        areaAnims[i].SetBool("isLocate", true);
                    }
                    else {
                        areaAnims[i].SetBool("isLocate", false);
                    }
                }
                break;

            case "Lake":
            case "Mountain_MidLake":
                for (int i = 0; i < areaAnims.Length; i++) {
                    if (i == 7) {
                        areaAnims[i].SetBool("isLocate", true);
                    }
                    else {
                        areaAnims[i].SetBool("isLocate", false);
                    }
                }
                break;

            case "Mushroom Cave":
            case "Mountain_Mushroom_Cave":
                for (int i = 0; i < areaAnims.Length; i++) {
                    if (i == 8) {
                        areaAnims[i].SetBool("isLocate", true);
                    }
                    else {
                        areaAnims[i].SetBool("isLocate", false);
                    }
                }
                break;

            case "Center Mountain 2":
            case "Mountain_Mid2":
                for (int i = 0; i < areaAnims.Length; i++) {
                    if (i == 9) {
                        areaAnims[i].SetBool("isLocate", true);
                    }
                    else {
                        areaAnims[i].SetBool("isLocate", false);
                    }
                }
                break;

            case "Out of Light":
                for (int i = 0; i < areaAnims.Length; i++) {
                    if (i == 10) {
                        areaAnims[i].SetBool("isLocate", true);
                    }
                    else {
                        areaAnims[i].SetBool("isLocate", false);
                    }
                }
                break;

            case "Top of Mountain":
            case "Mountain_Top":
                for (int i = 0; i < areaAnims.Length; i++) {
                    if (i == 11) {
                        areaAnims[i].SetBool("isLocate", true);
                    }
                    else {
                        areaAnims[i].SetBool("isLocate", false);
                    }
                }
                break;

            case "Dungeon":
                for (int i = 0; i < areaAnims.Length; i++) {
                    if (i == 12) {
                        areaAnims[i].SetBool("isLocate", true);
                    }
                    else {
                        areaAnims[i].SetBool("isLocate", false);
                    }
                }
                break;

            default:
                for (int i = 0; i < areaAnims.Length; i++) {
                    areaAnims[i].SetBool("isLocate", false);
                }
                break;
        }
    }
}