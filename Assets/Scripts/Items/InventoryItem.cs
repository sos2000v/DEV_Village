using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum ItemType
{
    Weapon,     // 검, 활 등 공격용
    Seed,       // 씨앗류
    Animal,     // 목축 관련
    Craft,      // 제작 재료
    Gift,       // 선물용 아이템
    Consumable  // 회복약 등
}

[System.Serializable]
public class InventoryItem
{
    public string itemName;   // 아이템 이름
    public Sprite icon;       // UI 아이콘
    public int maxStack = 99; // 최대 중첩 수
    public int amount = 1;    // 현재 개수

    public ItemType itemType;


    public InventoryItem(string name, int amount, Sprite icon = null, ItemType type = ItemType.Consumable, int maxStack = 99)
    {
        this.itemName = name;
        this.amount = amount;
        this.icon = icon;
        this.itemType = type;

        this.maxStack = maxStack;
    }
}
