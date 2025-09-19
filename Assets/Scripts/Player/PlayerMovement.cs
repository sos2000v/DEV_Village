using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float rotationSpeed = 10f;

    private Animator animator;
    private Rigidbody rb;

    private Vector3 moveDir;
    private bool isMoving;
    private bool isRunning;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();


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
}
