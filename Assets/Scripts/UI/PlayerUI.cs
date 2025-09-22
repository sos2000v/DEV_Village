using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerUI : MonoBehaviour
{
    public PlayerMovement playerMovement;
    public PlayerStamina playerStamina;

    public Slider healthBar;
    public Slider staminaBar;

    void Start()
    {
        healthBar.maxValue = playerMovement.maxHealth;
        staminaBar.maxValue = playerStamina.maxStamina;
        Debug.Log("[UI] 초기화 완료");
    }

    void Update()
    {
        healthBar.value = playerMovement.GetHealth();
        staminaBar.value = playerStamina.currentStamina;

       //Debug.Log($"[UI] 체력: {healthBar.value:F1}, 스태미나: {staminaBar.value:F1}");

        if (Input.GetKeyDown(KeyCode.H))
        {
            playerMovement.TakeDamage(20f);
            Debug.Log("[UI] H 키 눌러 체력 감소");
        }
    }
}
