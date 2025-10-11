using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEquipment : MonoBehaviour
{
    [Header("손에 들리는 위치")]
    public Transform handPoint; // 아이템을 들 위치

    private GameObject currentItemObject;

    // 🔹 장착 아이템 모델 표시
    public void EquipItemModel(InventoryItem item)
    {
        ClearEquippedItem(); // 기존 장착 제거

        if (item == null || item.itemSO == null || item.itemSO.prefab == null)
        {
            Debug.LogWarning("❌ 장착할 아이템 프리팹이 없음");
            return;
        }

        // 손 위치에 인스턴스화
        currentItemObject = Instantiate(item.itemSO.prefab, handPoint.position, handPoint.rotation, handPoint);
        Debug.Log($"👐 손에 든 아이템: {item.itemSO.itemName}");
    }

    // 🔹 장착 아이템 제거
    public void ClearEquippedItem()
    {
        if (currentItemObject != null)
        {
            Destroy(currentItemObject);
            currentItemObject = null;
            Debug.Log("[Equip] 장착 아이템 제거됨");
        }
    }
}
