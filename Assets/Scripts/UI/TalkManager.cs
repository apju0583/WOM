using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    public static TalkManager instance;
    Dictionary<int, string[]> talkData;

    void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            if (instance != this) {
                Destroy(this.gameObject);
            }
        }
        
        talkData = new Dictionary<int, string[]>();
        Generatedata();
    }

    void Generatedata() {
        talkData.Add(1000, new string[] { "안녕?", "처음 보는 얼굴이네.", "이곳에 새로운 사람이 온건 오랜만인 것 같아." });

        talkData.Add(2000, new string[] { "어서옵쇼!" });

        talkData.Add(3000, new string[] { "맡길 물건이 있나?" });

        talkData.Add(4000, new string[] { "이런..... 내 일 하나만 도와줄래?", "쓰고있던 도끼가 부러졌어.", "도끼 하나만 구해줄래?" });
        talkData.Add(4001, new string[] { "도끼는 상점에서 살 수 있어." });
        talkData.Add(4002, new string[] { "오, 고마워!", "작지만 부탁을 들어준 답례야." });
        talkData.Add(4003, new string[] { "앗 찾았다!", "미안해 이번에는 장작을 구해줄 수 있을까?" });
        talkData.Add(4004, new string[] { "장작도 상점에서 살 수 있어.", "아니면 산에 있는 나무에서도 구할 수 있어." });
        talkData.Add(4005, new string[] { "와 다행이다. 정말 고마워!", "여기 작지만 답례야." });
        talkData.Add(4006, new string[] { "이번 겨울은 걱정없겠다." });
    }

    public string GetTalk(int id, int talkIndex) {
        if (talkIndex == talkData[id].Length) {
            return null;
        }
        else {
            return talkData[id][talkIndex];
        }
    }
}
