using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class QuestManager : MonoBehaviour
{
    public static QuestManager instance;

    public Quest[] quests;
    public Text title;
    public Text detail;
    public Text request;

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

    void Start() 
    {
        title.text = "";
        detail.text = "";
        request.text = "";
    }

    public void Refresh() 
    {
        title.text = GameManager.instance.player.quest.questTitle;
        detail.text = GameManager.instance.player.quest.questDetail;
        Inventory inven = GameManager.instance.GetInventory().gameObject.GetComponent<Inventory>();
        int itemIndex = inven.items.IndexOf(quests[GameManager.instance.player.quest.questId].requestItem);
        int itemCnt = itemIndex == -1 ? 0 : inven.itemCounts[itemIndex];
        request.text = GameManager.instance.player.quest.requestItem.itemName + " (" + itemCnt + "/" + GameManager.instance.player.quest.requestCount + ")";
    }

    public void SetText() 
    {
        title.text = "";
        detail.text = "";
        request.text = "";
    }

    public void SetText(int index) 
    {
        title.text = quests[index].questTitle;
        detail.text = quests[index].questDetail;
    }

    public void Accept(int index) 
    {
        GameManager.instance.player.quest = quests[index];
        GameManager.instance.player.questStatus = false;
        GameManager.instance.player.clearStatus[index] = false;
        Debug.Log("퀘스트를 수락했습니다");
        Refresh();
    }

    public bool CheckClear(int index) 
    {
        Inventory inven = GameManager.instance.GetInventory().gameObject.GetComponent<Inventory>();
        int itemIndex = inven.items.IndexOf(quests[index].requestItem);

        if (itemIndex == -1) {
            return false;
        }

        return inven.itemCounts[itemIndex] >= quests[index].requestCount;
    }

    public void QuestClear(int index) 
    {
        if (CheckClear(index)) 
        {
            Inventory inven = GameManager.instance.GetInventory().gameObject.GetComponent<Inventory>();
            int itemIndex = inven.items.IndexOf(quests[index].requestItem);

            inven.RemoveItem(itemIndex, quests[index].requestCount);
            inven.AddGold(quests[index].reward);
            inven.FreshSlot();
            Debug.Log("\"" + quests[index].questTitle + "\"" + " 클리어!");
            GameManager.instance.player.quest = null;
            GameManager.instance.player.questStatus = false;
            GameManager.instance.player.clearStatus[index] = true;
            SetText();
        }
    }
}