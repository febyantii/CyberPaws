using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AgentPipMovement : MonoBehaviour
{
    [SerializeField] private float walkSpeed = 5f;
    [SerializeField] private float runSpeed = 8f;

    private Rigidbody2D rb;
    private Vector2 moveInput;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float moveX = 0f;
        float moveY = 0f;
        bool isRunning = false;

        // Cek sistem Input mana yang aktif di Unity kamu secara otomatis
        if (Keyboard.current != null)
        {
            // --- JIKA NEW INPUT SYSTEM AKTIF ---
            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) moveY += 1f;
            if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) moveY -= 1f;
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) moveX -= 1f;
            if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) moveX += 1f;

            isRunning = Keyboard.current.leftShiftKey.isPressed;
        }
        else
        {
            // --- JIKA OLD INPUT MANAGER AKTIF (FALLBACK AUTOMATIC) ---
            moveX = Input.GetAxisRaw("Horizontal");
            moveY = Input.GetAxisRaw("Vertical");

            isRunning = Input.GetKey(KeyCode.LeftShift);
        }

        moveInput = new Vector2(moveX, moveY).normalized;

        // Gerakkan Rigidbody2D
        float targetSpeed = isRunning ? runSpeed : walkSpeed;
        rb.velocity = moveInput * targetSpeed;

        // Atur Animasi & Flip Sprite
        bool isWalking = moveInput != Vector2.zero;
        animator.SetBool("isWalking", isWalking);
        animator.SetBool("isRunning", isRunning && isWalking);

        if (moveX < 0)
        {
            spriteRenderer.flipX = true;
        }
        else if (moveX > 0)
        {
            spriteRenderer.flipX = false;
        }

        if (isWalking)
        {
            animator.SetFloat("InputX", moveX);
            animator.SetFloat("InputY", moveY);
        }
    }
}