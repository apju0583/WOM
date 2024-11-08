using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager_First : MonoBehaviour
{
    public static GameManager_First instance;

    [Header("# Talk")]
    public Animator talkPanel;
    public TypeEffect talk;
    public GameObject scanObject;
    public int talkIndex;

    [Header("# Player Reference")]
    public Player_First player;

    public GameObject questPanel;
    public GameObject firstPanel;
    private Animator firstPanelAnimator;
    private bool inAdditionalDialogue = false;
    private bool questPanelShown = false;
    private bool finalDialogueReached = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        player = FindObjectOfType<Player_First>();

        if (talkPanel != null)
        {
            talkPanel.SetBool("isShow", false);
        }

        if (firstPanel != null)
        {
            firstPanelAnimator = firstPanel.GetComponent<Animator>();
            firstPanel.SetActive(false);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && firstPanel.activeSelf)
        {
            ToggleFirstPanel();
        }
    }

    public void ActivateFirstPanel()
    {
        if (firstPanel != null)
        {
            firstPanel.SetActive(true);
            firstPanelAnimator.SetBool("isShow", true);
        }
    }

    private void ToggleFirstPanel()
    {
        if (firstPanel != null)
        {
            bool isCurrentlyShown = firstPanelAnimator.GetBool("isShow");
            firstPanelAnimator.SetBool("isShow", !isCurrentlyShown);

            if (isCurrentlyShown)
            {
                StartCoroutine(DeactivateAfterAnimation(firstPanelAnimator, "Hide", firstPanel));
            }
        }
    }

    private IEnumerator DeactivateAfterAnimation(Animator animator, string animationState, GameObject panel)
    {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        panel.SetActive(false);
    }

    public void Action(GameObject scanObj)
    {
        if (questPanelShown)
        {
            CloseQuestPanel();
            inAdditionalDialogue = true;
            ContinueAdditionalDialogue();
            return;
        }

        if (inAdditionalDialogue)
        {
            ContinueAdditionalDialogue();
            return;
        }

        scanObject = scanObj;
        ObjData objData = scanObject.GetComponent<ObjData>();
        if (objData != null)
        {
            Talk(objData);
        }

        if (talkPanel != null)
        {
            talkPanel.SetBool("isShow", true);
        }
    }

    void Talk(ObjData data)
    {
        string talkData = "";

        if (talk.isAnim)
        {
            talk.SetMsg("");
            return;
        }
        
        else
        {
            talkData = TalkManager_First.instance.GetTalk(data.id, talkIndex);
        }

        if (talkData == null)
        {
            talkIndex = 0;
            talkPanel.SetBool("isShow", false);
            return;
        }

        talk.SetMsg(talkData);
        talkIndex += 1;
    }

    public void ShowQuestPanel()
    {
        if (questPanel != null)
        {
            questPanel.SetActive(true);
            questPanelShown = true;
        }
    }

    public void CloseQuestPanel()
    {
        if (questPanel != null)
        {
            questPanel.SetActive(false);
            questPanelShown = false;
        }
    }

    private void ContinueAdditionalDialogue()
    {
        string additionalMessage = TalkManager_First.instance.GetAdditionalDialogue();

        if (additionalMessage != null)
        {
            talkPanel.SetBool("isShow", true);
            talk.SetMsg(additionalMessage);

            if (additionalMessage == "할아버지...")
            {
                finalDialogueReached = true;
            }
        }

        else
        {
            inAdditionalDialogue = false;

            if (finalDialogueReached)
            {
                StartCoroutine(LoadVillageSceneWithLoading(3f));
                finalDialogueReached = false;
            }
        }
    }

    public void CloseTalkPanel()
    {
        if (talkPanel != null)
        {
            talkPanel.SetBool("isShow", false);
            talkIndex = 0;
        }
    }

    private IEnumerator LoadVillageSceneWithLoading(float delay)
    {
        yield return new WaitForSeconds(delay);
        LoadingSceneController.Instance.LoadScene("Village");
    }
}