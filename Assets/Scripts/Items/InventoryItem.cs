using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public enum ItemType
{
    None,       // 기본값
    Weapon,     // 공격용 무기 (검, 활, 도끼 등)
    Tool,       // 작업용 도구 (곡괭이, 낫, 물뿌리개 등)
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

    // ⚙️ 무기나 도구에만 해당하는 속성 (선택적으로 사용)
    public float attackPower;
    public float durability;


    public InventoryItem(string name, int amount, Sprite icon = null, ItemType type = ItemType.Consumable, int maxStack = 99)
    {
        this.itemName = name;
        this.amount = amount;
        this.icon = icon;
        this.itemType = type;
        this.maxStack = maxStack;
        this.attackPower = 0;
        this.durability = 100f;
    }
}
