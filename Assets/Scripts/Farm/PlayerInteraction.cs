using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class PlayerInteraction : MonoBehaviour
{
    PlayerMovement playerMovement;
    PlayerAttack playerAttack;
    PlayerInventory playerInventory;

    Land selectedLand = null;
    Camera mainCamera;


    void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        playerAttack = GetComponent<PlayerAttack>();
        playerInventory = GetComponent<PlayerInventory>();
        mainCamera = Camera.main; 
    }

    void Update()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition); // 마우스 위치에서 레이 생성
        RaycastHit hit;

        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);


        if (Physics.Raycast(ray, out hit, 100f)) // 100f는 최대 거리, 필요시 조절
        {
            OnInteractableHit(hit);
        }
        else if (selectedLand != null)
        {
            // 땅 선택 해제
            selectedLand.Select(false);
            selectedLand = null;
        }


        //좌클릭 동작
        if (Input.GetMouseButtonDown(0))
        {
            UseEquippedItem();
        }
    }


    void OnInteractableHit(RaycastHit hit)
    {
        Collider other = hit.collider;

        if (other.CompareTag("Land"))
        {
            Land land = other.GetComponent<Land>();
            SelectLand(land);
            //Debug.Log("마우스 위치의 농사 가능한 땅 선택");
        }
        else
        {
            // Land가 아닌 다른 곳이면 선택 해제
            if (selectedLand != null)
            {
                selectedLand.Select(false);
                selectedLand = null;
            }
        }
    }


    void SelectLand(Land land)
    {
        if (selectedLand != null)
        {
            selectedLand.Select(false);
        }

        selectedLand = land;
        land.Select(true);
    }


    void UseEquippedItem()
    {
        if (playerInventory == null || playerInventory.equippedItem == null) return;

        var item = playerInventory.equippedItem;

        switch (item.itemType)
        {
            case ItemType.Weapon:
                if (playerAttack != null)
                    playerAttack.Attack();
                break;

            case ItemType.Seed:
                if (selectedLand != null && selectedLand.landStatus == Land.LandStatus.Soil)
                {
                    // 땅을 경작 후 심기
                    selectedLand.SwitchLandStatus(Land.LandStatus.Farmland);
                    playerInventory.RemoveItem(item.itemName, 1);
                    Debug.Log($"🌱 {item.itemName} 심음!");
                }
                break;

            // 다른 타입도 추가 가능
            default:
                Debug.Log($"사용 불가 아이템: {item.itemName}");
                break;
        }
    }
}



