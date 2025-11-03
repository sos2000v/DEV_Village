using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInventory : MonoBehaviour
{
    [Header("인벤토리 설정")]
    public int slotCount = 27;
    public int hotbarCount = 9;

    public List<InventoryItem> items = new List<InventoryItem>();
    public ItemSO equippedItem;

    public delegate void OnInventoryChanged();
    public OnInventoryChanged onInventoryChangedCallback;

    // 🔹 핫바 아이템 장착
    public void EquipHotbarItem(int hotbarIndex)
    {
        PlayerEquipment playerEquip = FindObjectOfType<PlayerEquipment>();

        // 슬롯이 범위를 벗어나면 또는 아이템이 없으면 장착 해제
        if (hotbarIndex < 0 || hotbarIndex >= hotbarCount || hotbarIndex >= items.Count)
        {
            equippedItem = null;
            if (playerEquip != null)
                playerEquip.ClearEquippedItem();  // 빈 슬롯이면 모델 제거
            Debug.Log("[Equip] 빈 슬롯 선택, 장착 해제");
            return;
        }

        InventoryItem slotItem = items[hotbarIndex];   // InventoryItem 가져오기
        equippedItem = slotItem.itemSO;                // ItemSO 추출
        Debug.Log($"[Equip] {equippedItem.itemName} 장착");

        // PlayerEquipment에 InventoryItem 전달
        if (playerEquip != null)
            playerEquip.EquipItemModel(slotItem);
    }

    // 🔹 ItemSO 기반 아이템 추가
    public bool AddItem(ItemSO itemSO, int amount)
    {
        int remaining = amount;

        foreach (var item in items)
        {
            if (item.itemSO == itemSO && item.amount < item.maxStack)
            {
                int spaceLeft = item.maxStack - item.amount;
                int toAdd = Mathf.Min(spaceLeft, remaining);
                item.amount += toAdd;
                remaining -= toAdd;
                if (remaining <= 0) break;
            }
        }

        while (remaining > 0)
        {
            if (items.Count >= slotCount)
            {
                Debug.Log("❌ 인벤토리 가득 참!");
                return false;
            }

            int stackAmount = Mathf.Min(remaining, itemSO.maxStack);
            InventoryItem newItem = new InventoryItem(itemSO, stackAmount);
            items.Add(newItem);
            remaining -= stackAmount;
        }

        onInventoryChangedCallback?.Invoke();
        Debug.Log($"✅ 아이템 추가 완료: {itemSO.itemName} x{amount}");
        return true;
    }

    // 🔹 ItemSO 기반 아이템 제거
    public bool RemoveItem(ItemSO itemSO, int amount)
    {
        int remaining = amount;

        for (int i = items.Count - 1; i >= 0 && remaining > 0; i--)
        {
            if (items[i].itemSO == itemSO)
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
            Debug.LogWarning($"⚠️ {itemSO.itemName} {remaining}개 부족해서 전부 제거 못함");

        // ✅ 장착 중인 아이템이 전부 사라지면 자동 해제
        if (equippedItem == itemSO && !HasItem(itemSO))
        {
            equippedItem = null;

            PlayerEquipment playerEquip = FindObjectOfType<PlayerEquipment>();
            if (playerEquip != null)
                playerEquip.ClearEquippedItem();

            Debug.Log($"[Inventory] 장착 중인 {itemSO.itemName} 사라져서 자동 장착 해제됨");
        }

        onInventoryChangedCallback?.Invoke();
        Debug.Log($"🗑 아이템 제거: {itemSO.itemName} x{amount}");
        return true;
    }

    // 🔹 아이템 보유 여부
    public bool HasItem(ItemSO itemSO, int amount = 1)
    {
        int total = 0;
        foreach (var item in items)
            if (item.itemSO == itemSO)
                total += item.amount;
        return total >= amount;
    }
}
