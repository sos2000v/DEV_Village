using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro; 


public class InventorySlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI amountText; // ¡ç Text ¡æ TextMeshProUGUI·Î º¯°æ

    private InventoryItem item;

    public void AddItem(InventoryItem newItem)
    {
        item = newItem;
        icon.sprite = item.icon;
        icon.enabled = true;

        amountText.text = item.amount > 1 ? item.amount.ToString() : "";
    }

    public void ClearSlot()
    {
        item = null;
        icon.sprite = null;
        icon.enabled = false;
        amountText.text = "";
    }
}
