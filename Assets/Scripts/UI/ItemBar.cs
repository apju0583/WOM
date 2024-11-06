using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBar : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] Transform slotParent;
    [SerializeField] Slot[] slots;

    #if UNITY_EDITOR
        private void OnValidate() 
        {
            slots = slotParent.GetComponentsInChildren<Slot>();
        }
    #endif

    void Awake() 
    {
        RefreshSlot();
    }

    public void RefreshSlot() 
    {
        int i = 0;

        for (; i < inventory.items.Count && i < slots.Length; i++) 
        {
            slots[i].item = inventory.items[i];
        }

        for (; i < slots.Length; i++) 
        {
            slots[i].item = null;
        }
    }
}