using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public string startPointID;
    private Player thePlayer;
    private CameraManager theCamera;

    void Start()
    {
        thePlayer = FindObjectOfType<Player>();
        theCamera = FindObjectOfType<CameraManager>();

        if (startPointID == thePlayer.currentStartPointID)
        {
            thePlayer.transform.position = this.transform.position;
            theCamera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10f);
        }
    }
}