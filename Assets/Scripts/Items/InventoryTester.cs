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
            inventory.AddItem("당근", 1, carrotIcon);
        }

        if (Input.GetKeyDown(KeyCode.O))
        {
            inventory.RemoveItem("당근", 1);
        }
    }
}
