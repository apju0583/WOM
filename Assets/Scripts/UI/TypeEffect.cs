using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TypeEffect : MonoBehaviour
{
    public int charPerSeconds;
    public bool isAnim;
    string targetMsg;
    int index;
    float interval;

    Text msgText;
    public GameObject endCursor;
    AudioSource audioSource;

    void Awake() 
    {
        msgText = GetComponent<Text>();
        audioSource = GetComponent<AudioSource>();
    }

    public void SetMsg(string msg) 
    {
        if (isAnim) 
        {
            msgText.text = targetMsg;
            CancelInvoke();
            EffectEnd();
        }

        else 
        {
            targetMsg = msg;
            EffectStart();
        }
    }

    void EffectStart() 
    {
        msgText.text = "";
        index = 0;
        endCursor.SetActive(false);

        interval = 1.0f / charPerSeconds;
        isAnim = true;
        Invoke("Effecting", interval);
    }

    void Effecting() 
    {
        if (msgText.text == targetMsg) 
        {
            EffectEnd();
            return;
        }

        msgText.text += targetMsg[index];
        index += 1;
        Invoke("Effecting", interval);
    }

    void EffectEnd() 
    {
        isAnim = false;
        endCursor.SetActive(true);
    }
}