using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour
{
    public enum LandStatus
    {
        Soil,       // 기본 흙
        Farmland,   // 작물 심어진 상태
        Watered     // 물 준 상태
    }

    [Header("현재 상태")]
    public LandStatus landStatus;

    [Header("머티리얼 설정")]
    public Material soilMat;
    public Material wateredMat;

    [Header("작물 프리팹 (Farmland용)")]
    public GameObject cropPrefab;



    private Renderer renderer;
    private GameObject currentCrop; // 현재 심어진 작물



    [Header("선택 표시 오브젝트")]
    public GameObject select;

    void Start()
    {
        renderer = GetComponent<Renderer>();
        SwitchLandStatus(LandStatus.Soil);
        Select(false);
    }

    public void SwitchLandStatus(LandStatus statusToSwitch)
    {
        landStatus = statusToSwitch;

        switch (statusToSwitch)
        {
            case LandStatus.Soil:
                // 흙 머티리얼 적용
                if (renderer != null && soilMat != null)
                    renderer.material = soilMat;

                // 작물 제거
                RemoveCrop();
                break;

            case LandStatus.Farmland:
                // 머티리얼 교체 없음
                // 작물 생성
                SpawnCrop();
                break;

            case LandStatus.Watered:
                if (renderer != null && wateredMat != null)
                    renderer.material = wateredMat;

                // 작물은 유지
                break;
        }
    }

    // 선택 표시 토글
    public void Select(bool toggle)
    {
        if (select != null)
            select.SetActive(toggle);
    }

    // 작물 생성
    private void SpawnCrop()
    {
        if (cropPrefab == null) return;

        RemoveCrop();

        // Y 좌표 +2로 고정kslek
        Vector3 spawnPos = new Vector3(transform.position.x, 0f, transform.position.z);
        currentCrop = Instantiate(cropPrefab, spawnPos, Quaternion.identity, transform);
    }

    // 작물 제거
    private void RemoveCrop()
    {
        if (currentCrop != null)
        {
            Destroy(currentCrop);
            currentCrop = null;
        }
    }
}
