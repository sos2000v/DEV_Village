using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Crop : MonoBehaviour
{
    public int growthStage = 0;
    public GameObject[] growthPrefabs; // 각 단계별 모델/스프라이트
    public float timePerStage = 10f; // 성장 시간

    private float timer = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if (timer >= timePerStage && growthStage < growthPrefabs.Length - 1)
        {
            Grow();
            timer = 0f;
        }
    }

    public void Grow()
    {
        growthStage++;
        Debug.Log($"작물이 성장했다! 현재 단계: {growthStage}");

        foreach (Transform child in transform)
            Destroy(child.gameObject);

        Instantiate(growthPrefabs[growthStage], transform);
    }
}
