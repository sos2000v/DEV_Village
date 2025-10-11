using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("인벤토리 설정")]
    public int slotCount = 27;
    public int hotbarCount = 9;
    public List<InventoryItem> items = new List<InventoryItem>();

    public InventoryItem equippedItem;

    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;

    public bool AddItem(string name, int amount, ItemType type, Sprite icon = null)
    {
        int maxStack = GetDefaultMaxStack(type);
        int remaining = amount;

        // 1️⃣ 이미 있는 슬롯 중 같은 아이템 찾아서 순차적으로 채우기
        foreach (var item in items)
        {
            if (item.itemName == name && item.amount < item.maxStack)
            {
                int spaceLeft = item.maxStack - item.amount;
                int toAdd = Mathf.Min(spaceLeft, remaining);
                item.amount += toAdd;
                remaining -= toAdd;

                if (remaining <= 0)
                    break;
            }
        }

        // 2️⃣ 남은 양이 있으면 새로운 슬롯을 추가
        while (remaining > 0)
        {
            if (items.Count >= slotCount)
            {
                Debug.Log("❌ 인벤토리 가득 참!");
                return false;
            }

            int stackAmount = Mathf.Min(remaining, maxStack);
            InventoryItem newItem = new InventoryItem(name, stackAmount, icon, type, maxStack);
            newItem.itemType = type;

            items.Add(newItem);
            remaining -= stackAmount;
        }

        onInventoryChangedCallback?.Invoke();
        Debug.Log($"✅ 아이템 추가 완료: {name} x{amount}");
        return true;
    }

    // 아이템 제거
    public bool RemoveItem(string name, int amount)
    {
        int remaining = amount;

        for (int i = items.Count - 1; i >= 0 && remaining > 0; i--)
        {
            if (items[i].itemName == name)
            {
                if (items[i].amount > remaining)
                {
                    items[i].amount -= remaining;
                    remaining = 0;
                }
                else
                {
                    remaining -= items[i].amount;
                    items.RemoveAt(i);
                }
            }
        }

        if (remaining > 0)
            Debug.LogWarning($"⚠️ {name} {remaining}개 부족해서 전부 제거 못함");

        onInventoryChangedCallback?.Invoke();
        Debug.Log($"🗑 아이템 제거: {name} x{amount}");
        return true;
    }

    // 아이템 보유 여부
    public bool HasItem(string name, int amount = 1)
    {
        int total = 0;
        foreach (var item in items)
        {
            if (item.itemName == name)
                total += item.amount;
        }
        return total >= amount;
    }

    // 타입별 기본 최대 스택 설정
    private int GetDefaultMaxStack(ItemType type)
    {
        switch (type)
        {
            case ItemType.Tool: return 1;
            case ItemType.Consumable: return 10;
            case ItemType.Seed: return 99;
            default: return 99;
        }
    }
}
