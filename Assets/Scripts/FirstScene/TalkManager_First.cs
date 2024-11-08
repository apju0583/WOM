using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager_First : MonoBehaviour
{
    public static TalkManager_First instance;
    Dictionary<int, string[]> talkData;
    private string[] additionalDialogueSequence = {
        "이게 도대체 뭐지...?",
        "후.......",
        "할아버지..."
    };
    private int additionalDialogueIndex = 0;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            if (instance != this)
            {
                Destroy(this.gameObject);
            }
        }

        talkData = new Dictionary<int, string[]>();
        GenerateData();
    }

    void GenerateData()
    {
        talkData.Add(100, new string[] {
            "이 시간에 누구지...?",
            "...?",
            "편지가 떨어져있다.",
            "어딘지 모를 지도와 열쇠도 같이 있다...",
            "갑자기 이런 게 왜...?",
            "한 번 열어볼까.."
        });
    }

    public string GetTalk(int id, int talkIndex)
    {
        if (talkIndex == talkData[id].Length && id == 100)
        {
            GameManager_First.instance.ShowQuestPanel();
            return null;
        }

        if (talkIndex >= talkData[id].Length)
        {
            return null;
        }

        else
        {
            return talkData[id][talkIndex];
        }
    }

    public string GetAdditionalDialogue()
    {
        if (additionalDialogueIndex < additionalDialogueSequence.Length)
        {
            string message = additionalDialogueSequence[additionalDialogueIndex];
            additionalDialogueIndex++;
            return message;
        }
        else
        {
            additionalDialogueIndex = 0;
            return null;
        }
    }
}