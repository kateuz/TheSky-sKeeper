using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Rendering.Universal;

public class MesospherePlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpPower = 4f;
    [SerializeField] private float lowPressureJumpMultiplier = 0.5f;
    [SerializeField] private float lowPressureSpeedMultiplier = 0.7f;

    [Header("Heat Suit Settings")]
    [SerializeField] private float freezeThreshold = 0.3f;
    [SerializeField] private float freezeDamageRate = 0.1f;
    [SerializeField] private Image frostOverlay;
    [SerializeField] private float maxFrostAlpha = 0.8f;

    [Header("Meteor Detection")]
    [SerializeField] private GameObject meteorDetector;
    [SerializeField] private float meteorWarningTime = 2f;
    [SerializeField] private GameObject meteorWarningIndicator;

    private Rigidbody2D rb;
    private Animator animator;
    private float horizontalMovement;
    private bool isGrounded;
    private bool isFacingRight = true;
    private bool isHeatSuitActive = true;
    private bool isInLowPressureZone = false;
    private float currentFrostLevel = 0f;
    
    // Mobile control variables
    public bool left = false;
    public bool right = false;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        
        // Initialize UI elements
        if (frostOverlay != null)
        {
            Color color = frostOverlay.color;
            color.a = 0f;
            frostOverlay.color = color;
        }
    }

    private void Update()
    {
        // Handle movement input
        if (left)
        {
            horizontalMovement = -1f;
        }
        else if (right)
        {
            horizontalMovement = 1f;
        }
        else
        {
            horizontalMovement = Input.GetAxisRaw("Horizontal");
        }

        // Handle jump input
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            Jump();
        }

        // Handle heat suit toggle
        if (Input.GetKeyDown(KeyCode.H))
        {
            ToggleHeatSuit();
        }

        // Update frost effect if heat suit is off
        if (!isHeatSuitActive)
        {
            UpdateFrostEffect();
        }
    }

    private void FixedUpdate()
    {
        // Apply movement
        float currentSpeed = isInLowPressureZone ? moveSpeed * lowPressureSpeedMultiplier : moveSpeed;
        rb.velocity = new Vector2(horizontalMovement * currentSpeed, rb.velocity.y);
        
        // Update animation parameters
        if (animator != null)
        {
            animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
            animator.SetFloat("yVelocity", rb.velocity.y);
        }

        // Handle character flipping
        Flip();
    }

    private void Jump()
    {
        float jumpForce = isInLowPressureZone ? jumpPower * lowPressureJumpMultiplier : jumpPower;
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        if (animator != null)
        {
            animator.SetBool("isJumping", true);
        }
    }

    private void Flip()
    {
        if ((isFacingRight && horizontalMovement < 0f) || (!isFacingRight && horizontalMovement > 0f))
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    private void ToggleHeatSuit()
    {
        isHeatSuitActive = !isHeatSuitActive;
        if (isHeatSuitActive)
        {
            StartCoroutine(FadeFrostEffect(0f));
        }
    }

    private void UpdateFrostEffect()
    {
        currentFrostLevel = Mathf.Min(currentFrostLevel + freezeDamageRate * Time.deltaTime, 1f);
        if (frostOverlay != null)
        {
            Color color = frostOverlay.color;
            color.a = currentFrostLevel * maxFrostAlpha;
            frostOverlay.color = color;
        }

        // Apply movement penalties when freezing
        if (currentFrostLevel > freezeThreshold)
        {
            moveSpeed *= 0.8f;
        }
    }

    private IEnumerator FadeFrostEffect(float targetAlpha)
    {
        float duration = 0.5f;
        float startAlpha = frostOverlay.color.a;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / duration);
            Color color = frostOverlay.color;
            color.a = newAlpha;
            frostOverlay.color = color;
            yield return null;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            if (animator != null)
            {
                animator.SetBool("isJumping", false);
            }
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
        if (collision.CompareTag("LowPressureZone"))
        {
            isInLowPressureZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("LowPressureZone"))
        {
            isInLowPressureZone = false;
        }
    }

    // Call this method when a meteor is about to strike
    public void WarnMeteorStrike(Vector3 strikePosition)
    {
        if (meteorWarningIndicator != null)
        {
            StartCoroutine(ShowMeteorWarning(strikePosition));
        }
    }

    private IEnumerator ShowMeteorWarning(Vector3 position)
    {
        meteorWarningIndicator.transform.position = position;
        meteorWarningIndicator.SetActive(true);
        yield return new WaitForSeconds(meteorWarningTime);
        meteorWarningIndicator.SetActive(false);
    }

    // Mobile control methods
    public void Left()
    {
        left = true;
        right = false;
    }

    public void Right()
    {
        left = false;
        right = true;
    }

    public void Unpressed()
    {
        left = false;
        right = false;
    }
} 