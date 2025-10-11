using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class HotbarSelector : MonoBehaviour
{

    public int currentSlot = 0;
    public int maxSlots = 9;
    public Transform[] slots; // Slot_0 ~ Slot_8 연결

    [Header("선택 전환 설정")]
    public float scrollDelay = 0.2f; // 휠 스크롤 딜레이
    private float lastScrollTime = 0f;

    [Header("플레이어 손 위치")]
    public Transform handTransform; // 아이템이 들릴 위치
    private GameObject currentItemInHand;


    void Start()
    {
        // ✅ 게임 시작 시 기본 슬롯 1 선택
        currentSlot = 0;
        UpdateSelection();
        EquipSelectedItem(); // ✅ 시작 시 기본 아이템 장착

    }

    void Update()
    {
        int previousSlot = currentSlot;


        HandleMouseScroll();
        HandleNumberKeyInput();

        // 슬롯 변경 감지 → 아이템 갱신
        if (previousSlot != currentSlot)
        {
            UpdateSelection();
            EquipSelectedItem();
        }
    }

    void HandleMouseScroll()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) < 0.01f) return;

        // 🔹 딜레이 체크
        if (Time.time - lastScrollTime < scrollDelay) return;
        lastScrollTime = Time.time;

        // 🔹 슬롯 인덱스 변경
        if (scroll > 0f)
            currentSlot = (currentSlot + 1) % maxSlots;
        else if (scroll < 0f)
            currentSlot = (currentSlot - 1 + maxSlots) % maxSlots;


        // 🔹 즉시 반영
        UpdateSelection();
        EquipSelectedItem();
    }

    void HandleNumberKeyInput()
    {
        for (int i = 0; i < maxSlots; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                currentSlot = i;
                UpdateSelection();
                EquipSelectedItem(); // 숫자키도 즉시 반영
                break;
            }
        }
    }


    void UpdateSelection()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            // 선택된 슬롯 강조 (확대)
            slots[i].localScale = (i == currentSlot) ? Vector3.one * 1.2f : Vector3.one;

            // Highlight 이미지 활성화
            Transform highlight = slots[i].Find("Highlight");
            if (highlight != null)
                highlight.gameObject.SetActive(i == currentSlot);
        }

        //Debug.Log($"🎯 현재 선택 슬롯: {currentSlot + 1}");
    }

    void EquipSelectedItem()
    {
        // 🔸 기존 손 아이템 제거
        if (currentItemInHand != null)
            Destroy(currentItemInHand);

        // 🔹 PlayerInventory에서 실제 프리팹 불러오기
        PlayerInventory playerInv = FindObjectOfType<PlayerInventory>();
        if (playerInv != null)
        {
            playerInv.EquipHotbarItem(currentSlot);
        }


        GameObject selectedItem = GetSelectedItem();
        if (selectedItem != null)
        {
            // 🔸 손에 아이템 복제해서 붙이기
            GameObject itemPrefab = Instantiate(selectedItem, handTransform);
            itemPrefab.transform.localPosition = Vector3.zero;
            itemPrefab.transform.localRotation = Quaternion.identity;
            currentItemInHand = itemPrefab;

            Debug.Log($"🖐️ 손에 든 아이템 변경: {selectedItem.name}");
        }
        else
        {
            Debug.Log("❌ 선택된 아이템이 없습니다.");
        }
    }

    public GameObject GetSelectedItem()
    {
        return slots[currentSlot].childCount > 0
            ? slots[currentSlot].GetChild(0).gameObject
            : null;
    }
}
