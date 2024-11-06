using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu] public class Quest : ScriptableObject
{
    public int questId;
    public string questTitle;
    [TextArea] public string questDetail;
    public Item requestItem;
    public int requestCount;
    public int reward;
}