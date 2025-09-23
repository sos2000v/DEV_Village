using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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

    [Header("Control Flags")]
    //public bool canMove = true;
    public bool canControl = true; // 인벤토리 열렸을 때 이동 제한용
    bool isInventoryOpen = false; // 새 변수 추가


    [Header("Ground Detection")]
    public LayerMask groundLayer;  // 마우스 레이캐스트가 맞을 바닥 레이어
    public Camera mainCam;  // 🎯 인스펙터에서 직접 할당

    [HideInInspector]
    public bool isAttacking = false; // 공격 상태
    private PlayerAttack playerAttack;   // 공격 스크립트 참조




    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        playerStamina = GetComponent<PlayerStamina>();
        playerAttack = GetComponent<PlayerAttack>();

        currentHealth = maxHealth;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    void Update()
    {

        if (!canControl)
        {
            moveDir = Vector3.zero;
            animator.SetFloat("Speed", 0f);
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
            return;
        }

        // Tab 키로 인벤토리 열기/닫기
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            isInventoryOpen = !isInventoryOpen; // 인벤토리 상태 토글
            canControl = !isInventoryOpen;      // 인벤토리가 열려 있으면 이동 불가

            Cursor.lockState = canControl ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !canControl;

            // 공격 제한 동기화
            if (playerAttack != null)
                playerAttack.canControl = canControl;
        }

       
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
        isRunning = !exhausted && wantsToRun;

        animator.SetFloat("Speed", moveDir.magnitude);


        // 공격 중이면 마우스 방향으로 회전
        if (isAttacking && mainCam != null)
        {
            Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
            {
                Vector3 lookDir = hit.point - transform.position;
                lookDir.y = 0;
                if (lookDir.sqrMagnitude > 0.01f)
                {
                    Quaternion targetRot = Quaternion.LookRotation(lookDir);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
                }
            }
        }
    }


    void FixedUpdate()
    {
        if (!canControl) return;


        //// 마우스 커서 기준으로 회전 방향 구하기
        //Ray ray = mainCam.ScreenPointToRay(Input.mousePosition);
        //Debug.DrawRay(ray.origin, ray.direction * 100f, Color.red);

        //if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        //{
        //    //Debug.Log($"Hit at {hit.point}"); // 👈 이거 찍히는지 확인
        //    Vector3 lookDir = hit.point - transform.position;
        //    lookDir.y = 0;
        //    if (lookDir.sqrMagnitude > 0.01f)
        //    {
        //        Quaternion targetRot = Quaternion.LookRotation(lookDir);
        //        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        //    }
        //}
        ////else
        //{
        //    //Debug.Log("No ground hit!");
        //}

        //{

        //    Vector3 lookDir = hit.point - transform.position;
        //    lookDir.y = 0; // 수평 회전만
        //    if (lookDir.sqrMagnitude > 0.01f)
        //    {
        ////        Quaternion targetRot = Quaternion.LookRotation(lookDir);
        ////        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotationSpeed * Time.deltaTime);
        //    }
        //}

        if (moveDir.magnitude > 0 && !playerStamina.IsExhausted())
        {
            float currentSpeed = isRunning ? runSpeed : walkSpeed;
            rb.MovePosition(transform.position + moveDir * currentSpeed * Time.deltaTime);


            // 공격 중이 아니면 이동 방향 회전
            if (!isAttacking)
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDir);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }

            animator.SetBool("isWalking", !isRunning);
            animator.SetBool("isRunning", isRunning);
        }
        else
        {
            animator.SetBool("isWalking", false);
            animator.SetBool("isRunning", false);
        }
    }

    public void SetAttackState(bool state)
    {
        isAttacking = state;
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
