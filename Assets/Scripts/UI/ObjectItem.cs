using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectItem : MonoBehaviour, IObjectItem
{
    [Header("# 아이템")]
    public Item item;

    public Item ClickItem() 
    {
        return this.item;
    }

    public void OnClick() 
    {
        Inventory inventory = GameManager.instance.GetInventory().gameObject.GetComponent<Inventory>();
        
        inventory.AddItem(this.item);
        inventory.FreshSlot();
        GameManager.instance.GetItemBar().RefreshSlot();
    }
}