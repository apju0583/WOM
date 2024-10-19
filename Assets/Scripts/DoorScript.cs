using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DoorScript : MonoBehaviour
{
    public string transferMapName;
    public string startPointID;

    private Char thePlayer;
    private bool isPlayerInRange = false;
    private AudioSource audioSource;

    void Start()
    {
        thePlayer = FindObjectOfType<Char>();
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (isPlayerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (audioSource != null)
            {
                audioSource.Play();
            }

            thePlayer.currentMapName = transferMapName;
            thePlayer.currentStartPointID = startPointID;
            LoadingSceneController.Instance.LoadScene(transferMapName);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInRange = false;
        }
    }
}
