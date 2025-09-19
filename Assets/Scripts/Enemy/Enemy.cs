using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 50f;
    public float damageAmount = 10f;
    public float attackCooldown = 1f;
    public EnemyHealthBar healthBarPrefab; // Prefab 연결

    private float currentHealth;
    private float attackTimer = 0f;
    private bool isDead = false;
    private EnemyHealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;

        // HP바 생성
        if (healthBarPrefab != null && healthBar == null)
        {
            // HP바 생성 시 월드 공간에 바로 놓기
            healthBar = Instantiate(healthBarPrefab, transform.position, Quaternion.identity);
            healthBar.target = transform;
            healthBar.SetMaxHealth(maxHealth);
        }
    }

    void Update()
    {
        if (attackTimer > 0) attackTimer -= Time.deltaTime;

    }

    private void OnTriggerStay(Collider other)
    {
        if (isDead) return;

        if (other.CompareTag("Player") && attackTimer <= 0)
        {
            PlayerMovement player = other.GetComponent<PlayerMovement>();
            if (player != null)
            {
                player.TakeDamage(damageAmount);
                Debug.Log("몬스터가 플레이어 공격!");
                attackTimer = attackCooldown;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"몬스터 체력: {currentHealth}/{maxHealth}");


        if (healthBar != null)
            healthBar.SetHealth(currentHealth);

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        if (isDead) return;
        isDead = true;

        if (healthBar != null)
        {
            Destroy(healthBar.gameObject);
            healthBar = null; // null 처리
            Debug.Log($"{gameObject.name} 사망!");

        }

        Destroy(gameObject, 0.3f);
    }
}
