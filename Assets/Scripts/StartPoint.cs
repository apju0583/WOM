using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartPoint : MonoBehaviour
{
    public string startPointID;  // Start point ID to match the transfer point
    private Char thePlayer;
    private CameraManager theCamera;

    void Start()
    {
        thePlayer = FindObjectOfType<Char>();
        theCamera = FindObjectOfType<CameraManager>();

        // Check if this start point's ID matches the player's current start point ID
        if (startPointID == thePlayer.currentStartPointID)
        {
            // Move the player to this start point's position
            thePlayer.transform.position = this.transform.position;

            // Move the camera to follow the player, ensuring the z-value is set correctly for 2D (e.g., -10)
            theCamera.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -10f);
        }
    }
}
