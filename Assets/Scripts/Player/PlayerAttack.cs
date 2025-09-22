using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttack : MonoBehaviour
{
    public float attackDamage = 20f;
    public float attackRange = 2f;
    public float attackRadius = 1f;
    public LayerMask enemyLayer;
    public float attackStaminaCost = 15f;

    private Animator animator;
    private PlayerStamina playerStamina;

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerStamina = GetComponent<PlayerStamina>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Attack();
        }
    }

    private void Attack()
    {
        playerStamina.UseStamina(attackStaminaCost);

        if (playerStamina.IsExhausted())
        {
            Debug.Log("⚠ 지쳐서 공격 불가");
            return;
        }

        if (!playerStamina.HasStamina(attackStaminaCost))
        {
            Debug.Log("⚠ 스태미나 부족! 공격 불가");
            return;
        }

        animator.SetTrigger("Attack");

        Vector3 attackPoint = transform.position + transform.forward * attackRange;
        Collider[] hits = Physics.OverlapSphere(attackPoint, attackRadius, enemyLayer);

        foreach (Collider hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                Debug.Log($"플레이어가 {hit.name} 공격! (데미지 {attackDamage})");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = transform.position + transform.forward * attackRange;
        Gizmos.DrawWireSphere(center, attackRadius);
    }
}
