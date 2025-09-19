using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Stats")]
    public float maxHealth = 50f;
    public float damageAmount = 10f;
    public float attackCooldown = 1f;
    public EnemyHealthBar healthBarPrefab; // Prefab ����

    private float currentHealth;
    private float attackTimer = 0f;
    private bool isDead = false;
    private EnemyHealthBar healthBar;

    void Start()
    {
        currentHealth = maxHealth;

        // HP�� ����
        if (healthBarPrefab != null && healthBar == null)
        {
            // HP�� ���� �� ���� ������ �ٷ� ����
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
                Debug.Log("���Ͱ� �÷��̾� ����!");
                attackTimer = attackCooldown;
            }
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"���� ü��: {currentHealth}/{maxHealth}");


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
            healthBar = null; // null ó��
            Debug.Log($"{gameObject.name} ���!");

        }

        Destroy(gameObject, 0.3f);
    }
}
