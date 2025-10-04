using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [Header("UI Panel")]
    public GameObject slotGrid;          // 전체 인벤토리 (Tab으로 열고 닫음)
    public GameObject hotbarSlotGrid;    // 항상 켜져 있는 Hotbar

    private bool isOpen = false;

    public Transform slotsParent;        // 슬롯들이 들어갈 Grid
    private InventorySlot[] slots;
    private PlayerInventory inventory;
    private PlayerMovement player;       // 플레이어 제어
    private PlayerAttack playerAttack;   // 공격 제어 (있으면)

    void Start()
    {
        slotGrid.SetActive(false);  // 시작 시 전체 인벤토리 숨기기
        if (hotbarSlotGrid != null)
            hotbarSlotGrid.SetActive(true); // Hotbar는 항상 켜두기

        inventory = FindObjectOfType<PlayerInventory>();
        slots = slotsParent.GetComponentsInChildren<InventorySlot>();

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

            // 플레이어 움직임 제한
            if (player != null)
                player.canControl = !isOpen;
            Debug.Log($"[InventoryUI] canControl = {player.canControl}");


            // 공격 제한도 같이 처리
            if (playerAttack != null)
                playerAttack.canControl = !isOpen;

            // 마우스 커서 상태
            Cursor.lockState = isOpen ? CursorLockMode.None : CursorLockMode.Locked;
            Cursor.visible = isOpen;
        }
    }

    void UpdateUI()
    {
        // Hotbar 업데이트
        for (int i = 0; i < slots.Length; i++)
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
    }
}
