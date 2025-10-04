using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerInventory : MonoBehaviour
{
    [Header("인벤토리 설정")]
    public int slotCount = 27; // 인벤토리 칸 수
    public List<InventoryItem> items = new List<InventoryItem>();

    // 지금 장착된 아이템
    public InventoryItem equippedItem;

    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;

    public bool AddItem(string name, int amount, ItemType type, Sprite icon = null)
    {
        // 이미 있는 아이템 확인
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
                AddItem(name, leftover, type, icon);
            }
        }
        else
        {
            if (items.Count >= slotCount)
            {
                Debug.Log("❌ 인벤토리 가득 참!");
                return false;
            }

            // 새 아이템 생성
            InventoryItem newItem = new InventoryItem(name, amount, icon);
            newItem.itemType = type;

            // 새 아이템 우선순위: Hotbar에 넣기
            if (items.Count < hotbarCount)
            {
                // 핫바 끝 위치에 삽입
                items.Insert(items.Count, newItem);
            }
            else
            {
                // 일반 슬롯 뒤쪽으로 추가
                items.Add(newItem);
            }
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
