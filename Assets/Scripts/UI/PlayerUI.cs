using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    public PlayerMovement player;  // PlayerMovement 스크립트 연결
    public Slider healthBar;
    public Slider staminaBar;

    void Start()
    {
        // 시작 시 슬라이더 초기화
        healthBar.maxValue = player.maxHealth;
        staminaBar.maxValue = player.maxStamina;
    }

    void Update()
    {
        // PlayerMovement에서 현재 체력 / 스태미나 받아와 업데이트
        healthBar.value = player.GetHealth();
        staminaBar.value = player.GetStamina();
   
        healthBar.value = player.GetHealth();
        staminaBar.value = player.GetStamina();

        // 🔥 디버그: H 키 누르면 체력 감소
        if (Input.GetKeyDown(KeyCode.H))
        {
            player.TakeDamage(20f);
        }
    }



}
