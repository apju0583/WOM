using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public List<Item> items;
    public List<string> names;
    public List<int> itemCounts;
    public List<int> itemRank;
    public int gold;

    [SerializeField] Transform slotParent;
    [SerializeField] Slot[] slots; // 아이템 슬롯

    #if UNITY_EDITOR
        private void OnValidate() 
        {
            slots = slotParent.GetComponentsInChildren<Slot>();
        }
    #endif

    void Awake() 
    {
        for (int i = 0; i < items.Count; i++) 
        {
            names.Add(items[i].itemName);
            itemCounts.Add(1);
            itemRank.Add(items[i].rank);
        }
        FreshSlot();
    }

    public void FreshSlot() 
    {
        int i = 0;

        for (; i < items.Count && i < slots.Length; i++) 
        {
            slots[i].item = items[i];
            Text count = slots[i].gameObject.GetComponentInChildren<Text>();
            count.color = new Color(50/255f, 50/255f, 50/255f, 1);
            count.text = itemCounts[i].ToString();
        }

        for (; i < slots.Length; i++) 
        {
            slots[i].item = null;
            Text count = slots[i].gameObject.GetComponentInChildren<Text>();
            count.color = new Color(50/255f, 50/255f, 50/255f, 0);
        }

        Text[] goldText = GetComponentsInChildren<Text>();
        for (int j = 0; j < goldText.Length; j++) 
        {
            if (goldText[j].gameObject.CompareTag("Gold")) {
                goldText[j].text = gold.ToString();
            }
        }
    }

    public void AddItem(Item _item) 
    {
        if (items.Contains(_item)) 
        {
            int index = items.IndexOf(_item);
            itemCounts[index] += 1;
            Debug.Log("아이템이 추가되었습니다.");
            FreshSlot();
            if (GameManager.instance.player.quest != null) 
            {
                GameManager.instance.player.questStatus = QuestManager.instance.CheckClear(GameManager.instance.player.quest.questId);
                QuestManager.instance.Refresh();
            }
        }

        else 
        {
            if (items.Count < slots.Length) 
            {
                items.Add(_item);
                names.Add(_item.itemName);
                itemCounts.Add(1);
                itemRank.Add(_item.rank);
                Debug.Log("아이템이 추가되었습니다.");
                FreshSlot();
                if (GameManager.instance.player.quest != null) 
                {
                    GameManager.instance.player.questStatus = QuestManager.instance.CheckClear(GameManager.instance.player.quest.questId);
                    QuestManager.instance.Refresh();
                }
            }

            else 
            {
                Debug.Log("슬롯이 가득 찼습니다.");
            }
        }
    }

    public void RemoveItem(int index) 
    {
        if (items.Count != 0) 
        {
            itemCounts[index] -= 1;
            Debug.Log("아이템을 제거했습니다");

            if (itemCounts[index] == 0) 
            {
                items.RemoveAt(index);
                names.RemoveAt(index);
                itemCounts.RemoveAt(index);
                itemRank.RemoveAt(index);
                Debug.Log("아이템이 인벤토리에서 제거되었습니다");
            }

            FreshSlot();
            if (GameManager.instance.player.quest != null) 
            {
                GameManager.instance.player.questStatus = QuestManager.instance.CheckClear(GameManager.instance.player.quest.questId);
                QuestManager.instance.Refresh();
            }
        }

        else 
        {
            Debug.Log("제거에 실패했습니다");
        }
    }

    public void RemoveItem(int index, int cnt) 
    {
        if (items.Count != 0) {
            itemCounts[index] -= cnt;
            Debug.Log("아이템을 제거했습니다");

            if (itemCounts[index] <= 0) 
            {
                items.RemoveAt(index);
                names.RemoveAt(index);
                itemCounts.RemoveAt(index);
                itemRank.RemoveAt(index);
                Debug.Log("아이템이 인벤토리에서 제거되었습니다");
            }

            FreshSlot();
            if (GameManager.instance.player.quest != null) 
            {
                GameManager.instance.player.questStatus = QuestManager.instance.CheckClear(GameManager.instance.player.quest.questId);
                QuestManager.instance.Refresh();
            }
        }

        else 
        {
            Debug.Log("제거에 실패했습니다");
        }
    }

    public void UpgradeItem(int index) 
    {
        if (items.Count != 0 && items[index].type == Item.ItemType.equip) {
            itemRank[index] += 1;
            items[index].itemName = names[index] + " (+" + itemRank[index] + ")";
            Debug.Log("아이템이 강화되었습니다");
            FreshSlot();
        }

        else 
        {
            Debug.Log("강화에 실패했습니다");
        }
    }

    public void AddGold(int gold) 
    {
        this.gold += gold;
    }

    public void UseGold(int gold) 
    {
        this.gold -= gold;
    }
}