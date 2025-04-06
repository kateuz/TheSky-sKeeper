using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    float horizontalMovement;
    bool isFacingRight = true;
    bool isGrounded = false;
    int extraJumps;

    [SerializeField] int extraJumpsValue = 1;
    [SerializeField] float jumpPower = 4f;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] Rigidbody2D rb;
    Animator animator;

    public PlayerCombat playerCombat;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        extraJumps = extraJumpsValue;
    }

    void Update()
    {
        FLip();

        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded || extraJumps > 0)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpPower);
                animator.SetBool("isJumping", true);

                if (!isGrounded)
                {
                    extraJumps--;
                }
                
            }
        }

        if (Input.GetButtonDown("Slash"))
        {
            playerCombat.Slash();
        }
    }

    public void Move(InputAction.CallbackContext context)
    {
        horizontalMovement = context.ReadValue<Vector2>().x;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);
        animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
        animator.SetFloat("yVelocity", rb.velocity.y);
    }

    void FLip()
    {
        if ((isFacingRight && horizontalMovement < 0f) || (!isFacingRight && horizontalMovement > 0f))
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            extraJumps = extraJumpsValue;
            animator.SetBool("isJumping", false);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
