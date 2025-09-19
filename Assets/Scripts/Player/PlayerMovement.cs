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
    private bool isMoving;
    private bool isRunning;



    [Header("Player Stats")]
    public float maxHealth = 100f;
    public float maxStamina = 100f;
    public float staminaDrainPerSecond = 15f;
    public float staminaRecoveryPerSecond = 10f;
    public float staminaRecoveryDelay = 1f;

    private float currentHealth;
    private float currentStamina;
    private float staminaRecoveryTimer;



    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        // 초기화
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        staminaRecoveryTimer = 0f;


        // ✅ 캐릭터가 넘어지거나 벽 타지 않게 회전 고정
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {
        // 입력 받기
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        moveDir = new Vector3(h, 0, v).normalized;

        // 이동 여부
        isMoving = moveDir.magnitude > 0;
        isRunning = Input.GetKey(KeyCode.LeftShift) && isMoving;
        bool wantsToRun = Input.GetKey(KeyCode.LeftShift) && isMoving;



        // ✅ 스태미나 확인 (없으면 강제 걷기)
        if (wantsToRun && currentStamina > 0)
        {
            isRunning = true;
            currentStamina -= staminaDrainPerSecond * Time.deltaTime;
            staminaRecoveryTimer = 0f; // 회복 대기시간 초기화
        }
        else
        {
            isRunning = false;
            staminaRecoveryTimer += Time.deltaTime;
        }

        // ✅ 스태미나 회복
        if (!isRunning && staminaRecoveryTimer >= staminaRecoveryDelay)
        {
            currentStamina += staminaRecoveryPerSecond * Time.deltaTime;
            currentStamina = Mathf.Clamp(currentStamina, 0, maxStamina);
        }


        // ✅ Speed 파라미터 갱신 (Animator)
        animator.SetFloat("Speed", moveDir.magnitude);



    }




    void FixedUpdate()
    {
        if (isMoving)
        {
            // 이동
            float currentSpeed = isRunning ? runSpeed : walkSpeed;
            rb.MovePosition(transform.position + moveDir * currentSpeed * Time.deltaTime);

            // 회전
            Quaternion targetRotation = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            animator.SetBool("isWalking", !isRunning);
            animator.SetBool("isRunning", isRunning);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
    }


    // ✅ 외부에서 호출할 수 있는 체력 함수
    public void TakeDamage(float amount)
    {
        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);

        Debug.Log($"플레이어 피격! 남은 체력: {currentHealth}");


        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
    }

    private void Die()
    {
        Debug.Log("플레이어 사망!");
        // 여기서 사망 애니메이션, 리스폰, 게임오버 로직 등 추가 가능
    }

    // ✅ 현재 체력 / 스태미나 값 외부에서 참조 가능
    public float GetHealth() => currentHealth;
    public float GetStamina() => currentStamina;


}
