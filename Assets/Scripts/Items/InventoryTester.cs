using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    public PlayerInventory inventory;
    public ItemSO carrotItemSO; // ScriptableObject 참조

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            // ItemSO 기반으로 추가
            inventory.AddItem(carrotItemSO, 10);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            inventory.RemoveItem(carrotItemSO, 1);
        }
    }
}
