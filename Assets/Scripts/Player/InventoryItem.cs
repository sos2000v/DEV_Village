using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class InventoryItem
{
    public string itemName;   // 아이템 이름
    public Sprite icon;       // UI 아이콘
    public int maxStack = 99; // 최대 중첩 수
    public int amount = 1;    // 현재 개수

    public InventoryItem(string name, int amount, Sprite icon = null, int maxStack = 99)
    {
        this.itemName = name;
        this.amount = amount;
        this.icon = icon;
        this.maxStack = maxStack;
    }
}
