using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{

    public WeatherController weatherController;

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

    public bool left = false;
    public bool right = false;

    public TMP_Text wandMessage;
    public CanvasGroup wandMessageGroup;
    public string wandMessageTxt = "Weather Wand obtained!";

    public GameObject tempUp;
    public GameObject tempDown;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        extraJumps = extraJumpsValue;

        tempUp.SetActive(false);
        tempDown.SetActive(false);
    }

    void Update()
    {
        FLip();

        //if (Input.GetButtonDown("Jump"))
        //{
        //    if (isGrounded || extraJumps > 0)
        //    {
        //        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
        //        animator.SetBool("isJumping", true);

        //        if (!isGrounded)
        //        {
        //            extraJumps--;
        //        }

        //    }
        //}
        if (Input.GetButtonDown("Slash"))
        {
            playerCombat.Slash();
        }
    }
    
    public void Jump()
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

    public void Left()
    {
        left = true;
        right = false;
        Debug.Log("Left movement");
    }

    public void Right()
    {
        left = false;
        right = true;
        Debug.Log("Right movement");
    }

    public void Unpressed()
    {
        rb.velocity = Vector2.zero;
        left = false;
        right = false;
        Debug.Log("Stop");
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Wand") && collision.gameObject.activeSelf == true)
        {
            collision.gameObject.SetActive(false);

            wandMessage.text = wandMessageTxt;
            wandMessageGroup.gameObject.SetActive(true);

            if (weatherController != null)
            {
                weatherController.enabled = true;
                weatherController.AdjustTemperature(10f);
            }

            StartCoroutine(FadeMessage());

            tempUp.SetActive(true);
            tempDown.SetActive(true);
        }
    }

    IEnumerator FadeMessage()
    {
        float duration = 0.5f;
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            wandMessageGroup.alpha = Mathf.Lerp(0f, 1f, t/duration);
            yield return null;
        }

        wandMessageGroup.alpha = 1f;

        yield return new WaitForSeconds(2f);

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            wandMessageGroup.alpha = Mathf.Lerp(1f, 0f, t/duration);
            yield return null;
        }

        wandMessageGroup.alpha = 0f;
        wandMessageGroup.gameObject.SetActive(false);
    }
}
