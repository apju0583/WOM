using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransferMap : MonoBehaviour
{
    public string transferMapName;
    public string startPointID;

    private Player thePlayer;

    void Start()
    {
        thePlayer = FindObjectOfType<Player>();
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