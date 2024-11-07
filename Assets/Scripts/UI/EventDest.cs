using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventDest : MonoBehaviour
{
    private void Awake() 
    {
        if (FindObjectsOfType<EventSystem>().Length > 1) 
        {
            Destroy(gameObject);
        } 
        
        else 
        {
            DontDestroyOnLoad(gameObject);
        }
    }
}