using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public GameObject prefab; // ÇÁ¸®ÆÕ ¿¬°á
    public ItemType itemType;
    public int maxStack = 99;
    public Sprite icon;
}
