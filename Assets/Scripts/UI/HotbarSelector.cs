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

    void Start()
    {
        // ✅ 게임 시작 시 기본 슬롯 1 선택
        currentSlot = 0;
        UpdateSelection();
    }

    void Update()
    {
        HandleMouseScroll();
        HandleNumberKeyInput();
    }

    void HandleMouseScroll()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (Mathf.Abs(scroll) < 0.01f) return;

        if (Time.time - lastScrollTime < scrollDelay) return;

        if (scroll > 0f)
            currentSlot = (currentSlot + 1) % maxSlots;
        else if (scroll < 0f)
            currentSlot = (currentSlot - 1 + maxSlots) % maxSlots;

        UpdateSelection();
        lastScrollTime = Time.time;
    }

    void HandleNumberKeyInput()
    {
        for (int i = 0; i < maxSlots; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                currentSlot = i;
                UpdateSelection();
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

    public GameObject GetSelectedItem()
    {
        return slots[currentSlot].childCount > 0
            ? slots[currentSlot].GetChild(0).gameObject
            : null;
    }
}
