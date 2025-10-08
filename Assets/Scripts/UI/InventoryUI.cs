using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject slotGrid;          // 전체 인벤토리 (Tab으로 열고 닫음)
    public GameObject hotbarSlotGrid;    // 항상 켜져 있는 Hotbar

    private bool isOpen = false;

    [Header("Slot Parents")]
    public Transform slotsParent;        // 인벤토리 슬롯들 (SlotGrid)
    public Transform hotbarSlotsParent;  // 핫바 슬롯들 (Hotbar_SlotGrid)

    private InventorySlot[] slots;       // 전체 인벤토리 슬롯 배열
    private InventorySlot[] hotbarSlots; // 핫바 슬롯 배열

    private PlayerInventory inventory;
    private PlayerMovement player;
    private PlayerAttack playerAttack;

    void Start()
    {
        slotGrid.SetActive(false); // 시작 시 인벤토리 닫기
        if (hotbarSlotGrid != null)
            hotbarSlotGrid.SetActive(true); // 핫바는 항상 켜두기

        inventory = FindObjectOfType<PlayerInventory>();

        // 슬롯들 초기화
        slots = slotsParent.GetComponentsInChildren<InventorySlot>();
        hotbarSlots = hotbarSlotsParent.GetComponentsInChildren<InventorySlot>();

        player = FindObjectOfType<PlayerMovement>();
        playerAttack = FindObjectOfType<PlayerAttack>();

        inventory.onInventoryChangedCallback += UpdateUI;
        UpdateUI();
    }

    void Update()
    {
        // Tab 키 입력 시 인벤토리 토글
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isOpen = !isOpen;
            slotGrid.SetActive(isOpen);

            // 플레이어 이동 제어
            if (player != null)
                player.canControl = !isOpen;

            // 공격 제어
            if (playerAttack != null)
                playerAttack.canControl = !isOpen;

            // 마우스 커서 표시
            Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isOpen;
        }
    }

    void UpdateUI()
    {
        // 🔹 핫바 슬롯 갱신 (앞부분 N개)
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                hotbarSlots[i].AddItem(inventory.items[i]);
            }
            else
            {
                hotbarSlots[i].ClearSlot();
            }
        }

        // 🔹 일반 인벤토리 슬롯 갱신 (핫바 이후)
        for (int i = 0; i < slots.Length; i++)
        {
            int itemIndex = i + hotbarSlots.Length;
            if (itemIndex < inventory.items.Count)
            {
                slots[i].AddItem(inventory.items[itemIndex]);
            }
            else
            {
                slots[i].ClearSlot();
            }
        }
    }
}
