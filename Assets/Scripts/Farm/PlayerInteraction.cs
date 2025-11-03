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

    [Header("심기 딜레이")]
    public float plantDelay = 0.3f; // 초 단위
    private bool canPlant = true;

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


        // 우클릭 드래그 심기
        if (Input.GetMouseButton(1) && canPlant)
        {
            StartCoroutine(PlantWithDelay());
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

    IEnumerator PlantWithDelay()
    {
        canPlant = false; // 딜레이 동안 재호출 방지
        PlantSeed();
        yield return new WaitForSeconds(plantDelay);
        canPlant = true;
    }

    void PlantSeed()
    {
        // 손에 아이템이 없으면 바로 종료
        if (playerInventory == null || playerInventory.equippedItem == null)
        {
            Debug.Log("❌ 손에 아이템 없음, 심기 불가");
            return;
        }


        ItemSO item = playerInventory.equippedItem;

        if (item.itemType != ItemType.Seed)
        {
            Debug.Log("❌ 손에 씨앗을 들고 있어야 심을 수 있습니다!");
            return;
        }

        if (selectedLand != null && selectedLand.landStatus == Land.LandStatus.Soil)
        {
            // Land 상태 변경 → CropPrefab 생성
            selectedLand.SwitchLandStatus(Land.LandStatus.Farmland);

            // 인벤토리에서 씨앗 1개 제거
            bool removed = playerInventory.RemoveItem(item, 1);
            if (removed)
                Debug.Log($"🌱 {item.itemName} 심음!");
            else
                Debug.LogWarning($"❌ {item.itemName} 인벤토리에 없음");
        }
    }
}


