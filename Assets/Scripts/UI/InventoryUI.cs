using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{

    [Header("UI Panel")]
    public GameObject slotGrid; // 슬롯들을 담은 Grid
    private bool isOpen = false;


    public Transform slotsParent; // 슬롯들이 들어갈 Grid
    private InventorySlot[] slots;
    private PlayerInventory inventory;




    void Start()
    {

        slotGrid.SetActive(false);  // 시작 시 숨기기


        inventory = FindObjectOfType<PlayerInventory>();
        slots = slotsParent.GetComponentsInChildren<InventorySlot>();

        inventory.onInventoryChangedCallback += UpdateUI;
        UpdateUI();
    }


    void Update()
    {
        // Tab 키 입력 시 토글
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;
            slotGrid.SetActive(isOpen);

            // 플레이어 움직임 제한
            PlayerMovement player = FindObjectOfType<PlayerMovement>();
            if (player != null)
                player.canControl = !isOpen;

            // 게임 일시정지 옵션 (원하면 사용)
            // Time.timeScale = isOpen ? 0f : 1f;
        }
    }


    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[i]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
