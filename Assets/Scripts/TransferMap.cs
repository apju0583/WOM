using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    public string transferMapName;
    public string startPointID;

    private Char thePlayer;

    void Start()
    {
        thePlayer = FindObjectOfType<Char>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            thePlayer.currentMapName = transferMapName;
            thePlayer.currentStartPointID = startPointID;
            LoadingSceneController.Instance.LoadScene(transferMapName);
        }
    }
}