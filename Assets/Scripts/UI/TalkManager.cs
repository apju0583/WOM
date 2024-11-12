using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkManager : MonoBehaviour
{
    public static TalkManager instance;
    Dictionary<int, string[]> talkData;

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
        
        talkData = new Dictionary<int, string[]>();
        Generatedata();
    }

    void Generatedata() 
    {
        talkData.Add(4000, new string[] { "이전 퀘스트를 완료하고 와줘." });
        talkData.Add(4001, new string[] { "자넨 누군가?", "응? 편지에 있는 지도를 보고 왔다고?", "그게 무슨 소리여. 잠깐 줘봐", "아니 편지 뒤에 이 이름은...", 
                                         "내가 지도를 줄테니 M키를 눌러 확인 한 후 집으로 가보게", "그리고 집에 있는 상자를 찾아 열쇠를 가져오게", 
                                         "자세한 건 뒤에 얘기하지", "퀘스트 내용은 J를 눌러 다시 확인할 수 있다네"});
        talkData.Add(4002, new string[] { "아직 집으로 안갔나?"});
        talkData.Add(4003, new string[] { "그래.. 무슨 일인지 아는대로 말해주지", "오래전 자네의 할아버지가 사라지기 전에 나에게 자네가 올 것이라고 말했었네", 
                                         "마을에 닥친 위기를 해결하러 간다고 하고 떠난 후 다신 볼 수 없었지", 
                                         "이 열쇠가 그 증거라네", "자네는 앞으로 열쇠들을 모아 할아버지의 의지를 이을 사람이 될걸세",
                                         "열쇠는 어딘가 숨겨져있거나 특정 조건을 달성하면 얻을 수 있을걸세",
                                         "자네는 꼭 해낼 수 있을거라 믿네",
                                         "아 참, 위에 있는 상점가 쪽에 있는 청년에게 말을 걸어보세.",
                                         "자네가 사는데 도움을 줄 걸세", "그럼 또 도움이 필요하다면 찾아오게나"});
        talkData.Add(4004, new string[] { "마을의 평화를 부탁함세"});
        
        talkData.Add(150, new string[] { "이 상자인가?", "오 뭔가 있다", "이건 열쇠...?", "다시 마을로 돌아가보자"});
        talkData.Add(151, new string[] { "열쇠가 있던 상자다."});                           



        talkData.Add(8000, new string[] { "이전 퀘스트를 완료하고 와줘." });
        talkData.Add(8001, new string[] { "자네가 이장님이 말씀하신 청년인가?", "갑작스럽지만 만나게 되어서 반갑네",
                                          "열쇠들을 얻기 위해선 퀘스트들을 진행하며 성장을 하고", "맵 곳곳을 돌아다니며 숨겨진 것들을 찾아야 할 걸세",
                                          "그러기 위해서는 체력과 공격력 등을 강화해야 겠지", "왼쪽에 보이는 상점에서 무기와 방어구를 구매할 수 있네",
                                          "하지만 그럴려면 골드가 충분해야할 것이야", "우선 자네가 이 곳에서 살아남을 수 있을지 시험해봐야 겠군",
                                          "바로 위로 올라가다보면 슬라임들이 보일걸세", "슬라임을 처치하면 슬라임 액체라는 것이 나올 거야",
                                          "그걸 5개만 가져와보게.", "가끔 희귀한 확률로 특별한 아이템도 등장한다고 하는데",
                                          "그런 것들은 돈이 좀 된다고 하더군", "아무튼 슬라임 액체를 모아 다시 돌아오게" });
        talkData.Add(8002, new string[] { "슬라임을 처치하고 자격을 증명하세" });
        talkData.Add(8003, new string[] { "오호 이렇게나 빨리 돌아오다니", "생각보다 뛰어나구만",
                                          "아무튼 고생많았네. 여기 보상일세" });
        talkData.Add(8004, new string[] { "뭐 이정도면 어디가서 쉽게 죽진 않겠구만" });



        talkData.Add(6000, new string[] { "이전 퀘스트를 완료하고 와줘." });
        talkData.Add(6001, new string[] { "자네가 전 이장님의 손자인가..", "내가 오래 전 자네 할아버지에게 준 물건이 있는데",
                                          "그 물건이 지금 자네 할아버지의 묘에 있는 것 같네",
                                          "정말 미안하지만 가서 물건을 다시 가져와 줄 수 있겠나..?",
                                          "묘는 산 중부 1의 오른쪽 위 언덕에 있을 것이라네",
                                          "그럼 부탁하겠네.." });
        talkData.Add(6002, new string[] { "내가 몸이 불편해서,,, 잘 부탁하겠네" });
        talkData.Add(6003, new string[] { "그래 이 목걸이 일세...", "자네의 할아버지가 이장이 되었을 때 내가 선물해줬었지...",
                                          "갑자기 말도 없이 사라져서는...", "늙은이의 주책이였네, 부탁을 들어줘서 고맙네" });
        talkData.Add(6004, new string[] { "정말 고맙네 자네..." });

        talkData.Add(230, new string[] { "1번째 이장 <이 정 락>, 1997 ~ 2004"});
        talkData.Add(240, new string[] { "2번째 이장 <박 규 순>, 2005 ~ 2013"}); 
        talkData.Add(250, new string[] { "3번째 이장 <김 신 조>, 2014 ~ 2019", "!.. 여기가 할아버지의 묘...?", "뭔가 걸려있는 것 같다", "이건 목걸이...?"});
        talkData.Add(251, new string[] { "3번째 이장 <김 신 조>, 2014 ~ 2019"});    



        talkData.Add(350, new string[] { "여기서 약수를 얻을 수 있는 건가?", "굉장히 맑아보인다..."});
        talkData.Add(351, new string[] { "약수를 얻었으니 돌아가보자."});    

        talkData.Add(7000, new string[] { "이전 퀘스트를 완료하고 와줘." });
        talkData.Add(7001, new string[] { "혹시 정말 죄송한데 부탁하나만 드려도 될까요...?", "지금 저희 어머니가 많이 아프셔요",
                                          "산 호수에 있는 약수를 먹으면 회복에 도움이 된다는데",
                                          "너무 위험해서 제가 갈 수가 없어요", "혹시 약수를 구해와주실 수 있나요?",
                                          "꼭 부탁드리겠습니다." });
        talkData.Add(7002, new string[] { "산 호수는 중부 지역에서 왼쪽으로 가면 있다고 들었어요" });
        talkData.Add(7003, new string[] { "정말 생명의 은인이세요", "너무 감사합니다", "얼른 어머니한테 드릴게요 !!" });
        talkData.Add(7004, new string[] { "덕분에 어머니가 아픈 게 다 나았어요 !", "정말 감사합니다 !" });



        talkData.Add(2000, new string[] { "찾는 물건이 있나?"});        
        talkData.Add(3000, new string[] { "장비를 강화하고 싶나?"}); 



        talkData.Add(111, new string[] { "옆으로 가면 산으로 향할 수 있어", "어느 날 부터 산에 몬스터들이 나와서 위험하니 조심해" });
        talkData.Add(222, new string[] { "배고파..." });
        talkData.Add(333, new string[] { "바둑 한판 할래?" });
        talkData.Add(444, new string[] { "나랑 같이 놀래?" });
        talkData.Add(555, new string[] { "나른한 하루야.." });
        talkData.Add(999, new string[] { "어느새 열쇠를 모두 모아 여기까지 왔군", "아직 너가 모르는 것이 남아 있다", "너가 이제껏 헛고생을 했다는 것이지", "Comming Soon... (Early Access Ending)" });

        talkData.Add(123, new string[] {"상점"});
    }

    public string GetTalk(int id, int talkIndex) 
    {
        if (talkIndex == talkData[id].Length) 
        {
            return null;
        }

        else 
        {
            return talkData[id][talkIndex];
        }
    }
}