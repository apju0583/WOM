using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoadingSceneController : MonoBehaviour 
{
    private static LoadingSceneController instance;
    public static LoadingSceneController Instance 
    {
        get 
        {
            if (instance == null) 
            {
                var obj = FindObjectOfType<LoadingSceneController>();

                if (obj != null) 
                {
                    instance = obj;
                } 
                
                else 
                {
                    instance = Create();
                }
            }
            return instance;
        }
    }

    private static LoadingSceneController Create() 
    {
        return Instantiate(Resources.Load<LoadingSceneController>("LoadingUI"));
    }

    private void Awake() 
    {
        if (Instance != this) 
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private Image progressBar;
    [SerializeField] private TextMeshProUGUI loadingText;

    private string loadSceneName;

    [SerializeField] private List<string> loadingMessages;

    public void LoadScene(string sceneName) 
    {
        gameObject.SetActive(true);
        SceneManager.sceneLoaded += OnSceneLoaded;
        loadSceneName = sceneName;

        DisplayRandomMessage();

        StartCoroutine(LoadSceneProcess());
    }

    private IEnumerator LoadSceneProcess() 
    {
        progressBar.fillAmount = 0f;
        yield return StartCoroutine(Fade(true));

        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        op.allowSceneActivation = false;

        float timer = 0f;

        while (!op.isDone) 
        {
            yield return null;

            if (op.progress < 0.9f) 
            {
                progressBar.fillAmount = op.progress;
            } 
            
            else 
            {
                timer += Time.unscaledDeltaTime;
                progressBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);

                if (progressBar.fillAmount >= 1f) 
                {
                    op.allowSceneActivation = true;
                    yield break;
                }
            }
        }
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1) 
    {
        if (arg0.name == loadSceneName) 
        {
            StartCoroutine(Fade(false));
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    private void DisplayRandomMessage() 
    {
        if (loadingMessages.Count > 0) 
        {
            int randomIndex = UnityEngine.Random.Range(0, loadingMessages.Count);
            loadingText.text = loadingMessages[randomIndex];
        } 
        
        else 
        {
            loadingText.text = "Loading...";
        }
    }

    private IEnumerator Fade(bool isFadeIn) 
    {
        float timer = 0f;

        while (timer <= 1f) 
        {
            yield return null;
            timer += Time.unscaledDeltaTime * 3f;
            canvasGroup.alpha = isFadeIn ? Mathf.Lerp(0f, 1f, timer) : Mathf.Lerp(1f, 0f, timer);
        }

        if (!isFadeIn) 
        {
            gameObject.SetActive(false);
        }
    }
}