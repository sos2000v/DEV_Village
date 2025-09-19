using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float attackDamage = 20f;
    public float attackRange = 2f;
    public float attackRadius = 1f;
    public LayerMask enemyLayer;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("공격 시도!");
            Attack();
        }
    }

    private void Attack()
    {

        // 1️⃣ 애니메이션 재생
        if (animator != null)
        {
            animator.SetTrigger("Attack");
        }


        // 전방에 구체 범위로 탐색
        Vector3 attackPoint = transform.position + transform.forward * attackRange;
        Collider[] hits = Physics.OverlapSphere(attackPoint, attackRadius, enemyLayer);

        bool hitSomething = false;

        foreach (Collider hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(attackDamage);
                Debug.Log($"플레이어가 {hit.name} 공격! (데미지 {attackDamage})");
                hitSomething = true;
            }
        }

        if (!hitSomething)
        {
            Debug.Log("공격 범위 안에 몬스터 없음");
        }
    }

    // Scene 뷰에서 공격 범위 확인
    private void OnDrawGizmosSelected()
    {

        Gizmos.color = Color.red;
        Vector3 center = transform.position + transform.forward * attackRange;
        Gizmos.DrawWireSphere(center, attackRadius);
    }
}
