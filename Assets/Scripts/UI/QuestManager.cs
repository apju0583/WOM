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
        if (GameManager.instance.player.quest != null)
        {
            int questId = GameManager.instance.player.quest.questId;
            
            if (questId >= 0 && questId < quests.Length)
            {
                Quest currentQuest = quests[questId];
                title.text = currentQuest.questTitle;
                detail.text = currentQuest.questDetail;

                Inventory inven = GameManager.instance.GetInventory().gameObject.GetComponent<Inventory>();
                int itemIndex = inven.items.IndexOf(currentQuest.requestItem);
                int itemCnt = itemIndex == -1 ? 0 : inven.itemCounts[itemIndex];
                request.text = currentQuest.requestItem.itemName + " (" + itemCnt + "/" + currentQuest.requestCount + ")";
            }
            else
            {
                Debug.LogWarning("Assigned quest ID is out of bounds: " + questId);
                ClearUI();
            }
        }
        else
        {
            Debug.LogWarning("No quest assigned to the player.");
            ClearUI();
        }
    }

    private void ClearUI()
    {
        title.text = "";
        detail.text = "";
        request.text = "";
    }

    public void SetText()
    {
        ClearUI();
    }

    public void SetText(int index)
    {
        if (index >= 0 && index < quests.Length)
        {
            title.text = quests[index].questTitle;
            detail.text = quests[index].questDetail;
        }
    }

    public void Accept(int index)
    {
        if (index >= 0 && index < quests.Length)
        {
            GameManager.instance.player.quest = quests[index];
            GameManager.instance.player.questStatus = false;
            GameManager.instance.player.clearStatus[index] = false;
            Debug.Log("Quest accepted: " + quests[index].questTitle);
            Refresh();
        }
        else
        {
            Debug.LogWarning("Invalid quest index: " + index);
        }
    }

    public bool CheckClear(int index)
    {
        if (index >= 0 && index < quests.Length)
        {
            Inventory inven = GameManager.instance.GetInventory().gameObject.GetComponent<Inventory>();
            int itemIndex = inven.items.IndexOf(quests[index].requestItem);

            if (itemIndex == -1)
            {
                return false;
            }

            return inven.itemCounts[itemIndex] >= quests[index].requestCount;
        }
        Debug.LogWarning("Invalid quest index for CheckClear: " + index);
        return false;
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
            Debug.Log("Quest completed: " + quests[index].questTitle);
            GameManager.instance.player.quest = null;
            GameManager.instance.player.questStatus = false;
            GameManager.instance.player.clearStatus[index] = true;
            Debug.Log(GameManager.instance.player.clearStatus[index]);
            ClearUI();
        }
        else
        {
            Debug.LogWarning("Quest cannot be cleared, requirements not met.");
        }
    }
}