using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] public class Item : ScriptableObject
{
    public enum ItemType { atk, def, use, etc }

    public string itemName;
    public ItemType type;
    [TextArea] public string itemDesc;
    public int rank;
    public int price;
    public int ability;
    public Sprite itemImage;
}