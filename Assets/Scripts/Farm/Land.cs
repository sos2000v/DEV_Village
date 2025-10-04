using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Land : MonoBehaviour
{
    public enum LandStatus
    {
        Soil, Farmland, Watered
    }

    public LandStatus landStatus;

    public Material soilMat, farmlandMat, wateredMat;
    private Renderer renderer;

    public GameObject select;

    void Start()
    {
        // Renderer 컴포넌트 가져오기
        renderer = GetComponent<Renderer>();

        // 초기 상태 적용
        SwitchLandStatus(LandStatus.Soil);

        Select(false);

    }

    public void SwitchLandStatus(LandStatus statusToSwitch)
    {
        // 상태 저장
        landStatus = statusToSwitch;

        // 기본값 (Soil)
        Material materialToSwitch = soilMat;

        switch (statusToSwitch)
        {
            case LandStatus.Soil:
                materialToSwitch = soilMat;
                break;

            case LandStatus.Farmland:
                materialToSwitch = farmlandMat;
                break;

            case LandStatus.Watered:
                materialToSwitch = wateredMat;
                break;
        }

        // 머티리얼 교체
        renderer.material = materialToSwitch;
    }

    public void Select(bool toggle)
    { 
        select.SetActive(toggle);
    }

}

