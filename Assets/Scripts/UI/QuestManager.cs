using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public Quest[] quests;
    public GameObject[] questButtons;
    public Text title;
    public Text detail;

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
    }

    void Start() {
        int i = 0;
        for (; i < questButtons.Length && i < GameManager.instance.player.quests.Count; i++) {
            questButtons[i].SetActive(true);
            Text questTitle = questButtons[i].GetComponentInChildren<Text>();
            questTitle.text = GameManager.instance.player.quests[i].questTitle;
        }
        for (; i < questButtons.Length; i++) {
            questButtons[i].SetActive(false);
        }

        title.text = "";
        detail.text = "";
    }

    public void Refresh() {
        int i = 0;
        for (; i < questButtons.Length && i < GameManager.instance.player.quests.Count; i++) {
            questButtons[i].SetActive(true);
            Text questTitle = questButtons[i].GetComponentInChildren<Text>();
            questTitle.text = GameManager.instance.player.quests[i].questTitle;
        }
        for (; i < questButtons.Length; i++) {
            questButtons[i].SetActive(false);
        }
    }

    public void SetText() {
        title.text = "";
        detail.text = "";
    }

    public void SetText(int index) {
        title.text = quests[index].questTitle;
        detail.text = quests[index].questDetail;
    }

    public void Accept(int index) {
        GameManager.instance.player.quests.Add(quests[index]);
        GameManager.instance.player.questStatus.Add(false);
        Refresh();
        Debug.Log("퀘스트를 수락했습니다");
        // if (GameManager.instance.player.clearQuests.Last() == index - 1) {
        //     GameManager.instance.player.quests.Add(quests[index]);
        //     Refresh();
        //     Debug.Log("퀘스트를 수락했습니다.");
        // }
        // else {
        //     Debug.Log("퀘스트를 순서대로 진행해주세요");
        // }
    }

    public bool CheckClear(int index) {
        Inventory inven = GameManager.instance.GetInventory().gameObject.GetComponent<Inventory>();
        int itemIndex = inven.items.IndexOf(quests[index].requestItem);

        if (itemIndex == -1) {
            return false;
        }
        return inven.itemCounts[itemIndex] >= quests[index].requestCount;
    }

    public void QuestClear(int index) {
        if (CheckClear(index)) {
            Inventory inven = GameManager.instance.GetInventory().gameObject.GetComponent<Inventory>();
            int itemIndex = inven.items.IndexOf(quests[index].requestItem);

            inven.RemoveItem(itemIndex);
            inven.AddGold(quests[index].reward);
            inven.FreshSlot();
            Debug.Log("\"" + quests[index].questTitle + "\"" + " 클리어!");
            Refresh();
        }
    }
}
