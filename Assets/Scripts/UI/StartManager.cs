using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartManager : MonoBehaviour
{
    public GameObject logo;
    public GameObject startText;
    public Text flashText;
    public GameObject buttons;
    public GameObject optionPanel;

    void Awake() 
    {
        logo.SetActive(true);
        startText.SetActive(true);
        buttons.SetActive(false);
        flashText = startText.GetComponentInChildren<Text>();
    }

    void Start() 
    {
        StartCoroutine(BlinkText());
    }

    void Update() 
    {
        if (Input.anyKeyDown) 
        {
            StopCoroutine(BlinkText());
            logo.SetActive(false);
            startText.SetActive(false);
            buttons.SetActive(true);
        }
    }

    public void ChangeScene() 
    {
        LoadingSceneController.Instance.LoadScene("First_Scene");
    }

    public void ExitGame() 
    {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }

    public void ShowOption() 
    {
        optionPanel.SetActive(true);
    }

    public void HideOption() 
    {
        optionPanel.SetActive(false);
    }

    IEnumerator BlinkText() 
    {
        while (true) {
            string text = flashText.text;
            flashText.text = "";
            yield return new WaitForSeconds(0.5f);
            flashText.text = text;
            yield return new WaitForSeconds(0.5f);
        }
    }
}