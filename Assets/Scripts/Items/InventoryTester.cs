using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTester : MonoBehaviour
{
    public PlayerInventory inventory;
    public Sprite carrotIcon;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            // Seed 타입 아이템으로 추가
            inventory.AddItem("당근", 10, ItemType.Seed, carrotIcon);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            inventory.RemoveItem("당근", 1);
        }
    }
}
