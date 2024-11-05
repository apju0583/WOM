using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyObject : MonoBehaviour {
    static public DontDestroyObject instance; // Public static reference to the instance

    void Awake() {
        // if (instance != null) {
        //     Debug.Log("Destroying duplicate Canvas: " + this.gameObject.name);
        //     Destroy(this.gameObject); // Destroy duplicate instances
        //     return;
        // }

        // DontDestroyOnLoad(this.gameObject); // Make this GameObject persistent
        // instance = this; // Set the instance
        // Debug.Log("DontDestroyObject instance created: " + this.gameObject.name);
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            if (instance != this) {
                Destroy(this.gameObject);
            }
        }
    }
}
