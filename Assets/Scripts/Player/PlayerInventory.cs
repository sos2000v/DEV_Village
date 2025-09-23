using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerInventory : MonoBehaviour
{
    [Header("인벤토리 설정")]
    public int slotCount = 20; // 인벤토리 칸 수
    public List<InventoryItem> items = new List<InventoryItem>();

    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;

    // 아이템 추가
    public bool AddItem(string name, int amount, Sprite icon = null)
    {
        // 이미 있는 아이템인지 확인
        InventoryItem existingItem = items.Find(i => i.itemName == name);

        if (existingItem != null)
        {
            if (existingItem.amount + amount <= existingItem.maxStack)
            {
                existingItem.amount += amount;
            }
            else
            {
                int leftover = (existingItem.amount + amount) - existingItem.maxStack;
                existingItem.amount = existingItem.maxStack;
                AddItem(name, leftover, icon); // 남은 것 재귀 추가
            }
        }
        else
        {
            if (items.Count >= slotCount)
            {
                Debug.Log("❌ 인벤토리 가득 참!");
                return false;
            }

            items.Add(new InventoryItem(name, amount, icon));
        }

        onInventoryChangedCallback?.Invoke();
        Debug.Log($"✅ 아이템 추가: {name} x{amount}");
        return true;
    }

    // 아이템 제거
    public bool RemoveItem(string name, int amount)
    {
        InventoryItem existingItem = items.Find(i => i.itemName == name);

        if (existingItem == null) return false;

        existingItem.amount -= amount;
        if (existingItem.amount <= 0)
        {
            items.Remove(existingItem);
        }

        onInventoryChangedCallback?.Invoke();
        Debug.Log($"🗑 아이템 제거: {name} x{amount}");
        return true;
    }

    // 특정 아이템 보유 여부 확인
    public bool HasItem(string name, int amount = 1)
    {
        InventoryItem existingItem = items.Find(i => i.itemName == name);
        return existingItem != null && existingItem.amount >= amount;
    }
}
