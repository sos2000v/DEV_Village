using JetBrains.Annotations;
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
    private PlayerMovement playerMovement;
    private PlayerInventory playerInventory;



    [Header("Control Flag")]
    public bool canControl = true; // 인벤토리 열면 공격 제한

    void Awake()
    {
        animator = GetComponent<Animator>();
        playerStamina = GetComponent<PlayerStamina>();
        playerMovement = GetComponent<PlayerMovement>(); // PlayerMovement 참조
        playerInventory = GetComponent<PlayerInventory>(); // ✅ 인벤토리 가져오기
    }

    void Update()
    {
        if (!canControl) return;

        if (Input.GetMouseButtonDown(0))
        {
            animator.ResetTrigger("Attack"); // 이전 트리거 초기화
            Attack();
        }
    }

    public void Attack()
    {
        // ✅ 1️ 검 장착 여부 확인
        if (playerInventory == null || playerInventory.equippedItem == null)
        {
            Debug.Log("⚠ 무기가 없습니다! (검을 장착해야 공격 가능)");
            return;
        }

        // ✅ 2️ 이름 또는 타입으로 검인지 확인
        if (playerInventory.equippedItem.itemName != "검" &&
            playerInventory.equippedItem.itemType != ItemType.Weapon)
        {
            Debug.Log("⚠ 검을 들어야 공격할 수 있습니다!");
            return;
        }

        // ✅ 3️ 스태미나 체크
        if (!playerStamina.HasStamina(attackStaminaCost))
        {
            Debug.Log("⚠ 스태미나 부족! 공격 불가");
            return;
        }

        playerStamina.UseStamina(attackStaminaCost);

        if (playerStamina.IsExhausted())
        {
            Debug.Log("⚠ 지쳐서 공격 불가");
            return;
        }

        // ✅ 4️ 공격 시작: PlayerMovement에 알리기
        if (playerMovement != null)
            playerMovement.isAttacking = true;


        animator.ResetTrigger("Attack");
        animator.SetTrigger("Attack");
    }


        // 애니메이션 이벤트에서 호출할 함수
    public void ApplyDamage()
    {
            
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

    // 애니메이션 이벤트: 공격 종료 시점
    public void OnAttackEnd()
    {
        if (playerMovement != null)
            playerMovement.SetAttackState(false);
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector3 center = transform.position + transform.forward * attackRange;
        Gizmos.DrawWireSphere(center, attackRadius);
    }
}