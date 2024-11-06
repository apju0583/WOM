using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SaveData
{
    public float playerPositionX;
    public float playerPositionY;
    public float playerPositionZ;
    public List<string> inventoryItems;
    public List<int> inventoryCounts;
    public int gold;
    public int questId;
    public bool questStatus;
}