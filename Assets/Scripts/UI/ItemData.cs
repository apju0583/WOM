using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemData : MonoBehaviour
{
    public int id;

    public void OnClick() 
    {
        GameManager.instance.itemId = id;
        GameManager.instance.ShowSelect();
    }

    public void OnClick2() 
    {
        GameManager.instance.itemId = id;
        GameManager.instance.ShowSelect2();
    }

    public void OnClick3() 
    {
        GameManager.instance.itemId = id;
        GameManager.instance.ShowSelect3();
    }
}