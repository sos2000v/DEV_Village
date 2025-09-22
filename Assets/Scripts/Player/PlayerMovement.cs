using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float rotationSpeed = 10f;

    private Animator animator;
    private Rigidbody rb;
    private Vector3 moveDir;
    private bool isRunning;

    private PlayerStamina playerStamina;
    private bool wasExhausted = false;

    [Header("Player Stats")]
    public float maxHealth = 100f;
    private float currentHealth;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerStamina = GetComponent<PlayerStamina>();

        currentHealth = maxHealth;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // 이동 입력
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveDir = new Vector3(h, 0, v).normalized;
        bool wantsToRun = Input.GetKey(KeyCode.LeftShift) && moveDir.magnitude > 0;

        // Exhausted 체크
        bool exhausted = playerStamina.IsExhausted();
        if (exhausted != wasExhausted)
        {
            animator.SetBool("isExhausted", exhausted);
            wasExhausted = exhausted;
            if (exhausted)
                Debug.Log("⚠ 지쳐서 움직일 수 없음!");
        }

        // 달리기/걷기
        if (!exhausted && wantsToRun)
        {
            isRunning = true;
        }
        else
        {
            isRunning = false;
        }

        animator.SetFloat("Speed", moveDir.magnitude);
    }

    void FixedUpdate()
    {
        // 이동 방향이 있고, 지치지 않았을 때만 이동/회전
        if (moveDir.magnitude > 0 && !playerStamina.IsExhausted())
        {
            float currentSpeed = isRunning ? runSpeed : walkSpeed;
            rb.MovePosition(transform.position + moveDir * currentSpeed * Time.deltaTime);

            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            animator.SetBool("isWalking", !isRunning);
            animator.SetBool("isRunning", isRunning);
        }
        else
        {
            // 이동이나 회전 모두 멈춤
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }

    }

    public float GetHealth() => currentHealth;

    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"[Movement] 체력 감소: {amount}, 남은 체력: {currentHealth}");

        if (currentHealth <= 0)
            Die();
    }

    private void Die()
    {
        Debug.Log("☠ 플레이어 사망!");
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"[Movement] 체력 회복: {amount}, 현재 체력: {currentHealth}");
    }
}
