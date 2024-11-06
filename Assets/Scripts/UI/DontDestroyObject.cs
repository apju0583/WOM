using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObject : MonoBehaviour 
{
    static public DontDestroyObject instance;

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
    }
}