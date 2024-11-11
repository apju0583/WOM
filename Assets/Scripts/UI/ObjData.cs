using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjData : MonoBehaviour
{
    public enum NPCType { normal, merchant, blacksmith, quest }

    public int id;
    public Item itemToGive;
    public bool isNPC;
    public NPCType type;
    public int[] questId;
    public int questIndex;
}