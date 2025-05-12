using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MesospherePlayerMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float jumpPower = 4f;
    [SerializeField] private float lowPressureJumpMultiplier = 0.5f;
    [SerializeField] private float lowPressureSpeedMultiplier = 0.7f;

    [Header("Frost Zone Settings")]
    [SerializeField] private float frostZoneMoveSpeedMultiplier = 0.5f;
    [SerializeField] private float frostZoneJumpMultiplier = 0.5f;
    [SerializeField] private float frostFadeInDuration = 1f;
    [SerializeField] private float minTemperatureToExit = 50f; // Temperature needed to exit the frost zone

    [Header("Frost Settings")]
    //[SerializeField] private float freezeThreshold = 0.3f;
    //[SerializeField] private float freezeDamageRate = 0.1f;
    [SerializeField] private Image frostOverlay;
    [SerializeField] private float maxFrostAlpha = 0.8f;
    [SerializeField] private float initialFrostLevel = 0.7f; // Start with high frost
    [SerializeField] private float temperatureChangeRate = 0.2f; // How fast temperature changes affect frost

    [Header("Meteor Detection")]
    [SerializeField] private GameObject meteorDetector;
    [SerializeField] private float meteorWarningTime = 2f;
    [SerializeField] private GameObject meteorWarningIndicator;

    [Header("Meteor Shield")]
    [SerializeField] private GameObject meteorShield;
    public bool isShieldActive { get; private set; } = false;

    [Header("Weather Wand")]
    [SerializeField] private GameObject weatherWandEffect;
    [SerializeField] private float wandWarmthRate = 0.2f;
    [SerializeField] private float wandCooldown = 5f;
    public float wandTimer = 0f;
    public bool isWandActive { get; private set; } = false;

    [Header("Weather System")]
    [SerializeField] private WeatherController weatherController;

    private Rigidbody2D rb;
    private Animator animator;
    private float horizontalMovement;
    private bool isGrounded;
    private bool isFacingRight = true;
    private bool isHeatSuitActive = true;
    public bool left = false;
    public bool right = false;
    private float currentFrostLevel = 0f;

    // Frost zone state
    private bool isInFrostZone = false;
    private bool isFrostZoneDefrosted = false;
    private float originalMoveSpeed;
    private float originalJumpPower;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // Store original movement values
        originalMoveSpeed = moveSpeed;
        originalJumpPower = jumpPower;

        // Initialize UI elements
        if (frostOverlay != null)
        {
            // Start with frost overlay hidden
            frostOverlay.gameObject.SetActive(false);
            frostOverlay.raycastTarget = false;
            Color color = frostOverlay.color;
            color.a = 0f;
            frostOverlay.color = color;
        }
        else
        {
            Debug.LogError("Frost Overlay Image is not assigned in the inspector!");
        }

        // Initialize shield
        if (meteorShield != null)
        {
            meteorShield.SetActive(false);
        }

        // Initialize weather controller
        if (weatherController == null)
        {
            weatherController = FindObjectOfType<WeatherController>();
            if (weatherController == null)
            {
                Debug.LogError("WeatherController not found in scene!");
            }
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

        // Update frost effect if in frost zone
        if (isInFrostZone)
        {
            UpdateFrostEffect();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("FrostZone"))
        {
            EnterFrostZone();
        }
        else if (collision.CompareTag("ShieldPickup"))
        {
            ActivateMeteorShield();
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("FrostZone"))
        {
            if (!isFrostZoneDefrosted)
            {
                // Prevent exit if not defrosted
                transform.position = collision.transform.position;
            }
            else
            {
                ExitFrostZone();
            }
        }
    }

    private void EnterFrostZone()
    {
        isInFrostZone = true;
        isFrostZoneDefrosted = false;
        
        // Apply frost zone movement restrictions
        moveSpeed = originalMoveSpeed * frostZoneMoveSpeedMultiplier;
        jumpPower = originalJumpPower * frostZoneJumpMultiplier;
        
        // Show and start frost effect
        if (frostOverlay != null)
        {
            frostOverlay.gameObject.SetActive(true);
            StartCoroutine(FadeFrostEffect(maxFrostAlpha));
        }
    }

    private void ExitFrostZone()
    {
        isInFrostZone = false;
        
        // Restore original movement values
        moveSpeed = originalMoveSpeed;
        jumpPower = originalJumpPower;
        
        // Hide frost effect
        if (frostOverlay != null)
        {
            StartCoroutine(FadeFrostEffect(0f));
            // Deactivate the overlay after fade out
            StartCoroutine(DeactivateFrostOverlay());
        }
    }

    private IEnumerator DeactivateFrostOverlay()
    {
        yield return new WaitForSeconds(0.5f); // Wait for fade out to complete
        if (frostOverlay != null)
        {
            frostOverlay.gameObject.SetActive(false);
        }
    }

    private void ActivateMeteorShield()
    {
        isShieldActive = true;
        if (meteorShield != null)
        {
            meteorShield.SetActive(true);
        }
        // Make meteors semi-transparent
        GameObject[] meteors = GameObject.FindGameObjectsWithTag("Meteor");
        foreach (GameObject meteor in meteors)
        {
            SpriteRenderer sr = meteor.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color color = sr.color;
                color.a = 0.5f; // Make meteors semi-transparent
                sr.color = color;
            }
        }
    }

    public void DeactivateMeteorShield()
    {
        isShieldActive = false;
        if (meteorShield != null)
        {
            meteorShield.SetActive(false);
        }
        // Restore meteor opacity
        GameObject[] meteors = GameObject.FindGameObjectsWithTag("Meteor");
        foreach (GameObject meteor in meteors)
        {
            SpriteRenderer sr = meteor.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color color = sr.color;
                color.a = 1f;
                sr.color = color;
            }
        }
    }

    public void ActivateWeatherWand()
    {
        isWandActive = true;
        wandTimer = wandCooldown;
        if (weatherWandEffect != null)
        {
            weatherWandEffect.SetActive(true);
        }
        
        if (isInFrostZone)
        {
            // Increase temperature more in frost zone
            if (weatherController != null)
            {
                weatherController.AdjustTemperature(wandWarmthRate * 2f);
            }
        }
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

    private void FixedUpdate()
    {
        // Apply movement without low pressure check
        rb.velocity = new Vector2(horizontalMovement * moveSpeed, rb.velocity.y);

        // Update animation parameters
        if (animator != null)
        {
            animator.SetFloat("xVelocity", Mathf.Abs(rb.velocity.x));
            animator.SetFloat("yVelocity", rb.velocity.y);
        }

        // Handle character flipping
        Flip();
    }

    public void Jump()
    {
        // Remove low pressure jump multiplier
        rb.velocity = new Vector2(rb.velocity.x, jumpPower);
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
        Debug.Log($"Heat suit {(isHeatSuitActive ? "activated" : "deactivated")}. Current frost level: {currentFrostLevel}");
    }

    private void UpdateFrostEffect()
    {
        if (frostOverlay != null && weatherController != null)
        {
            // Map temperature from -20 to 100 to frost level from 1 to 0
            float normalizedTemp = Mathf.InverseLerp(-20f, 100f, weatherController.currentTemperature);
            currentFrostLevel = Mathf.Lerp(1f, 0f, normalizedTemp);
            
            Color color = frostOverlay.color;
            color.a = currentFrostLevel * maxFrostAlpha;
            frostOverlay.color = color;
            Debug.Log($"Frost level: {currentFrostLevel}, Alpha: {color.a}");

            // Check if zone is defrosted
            if (weatherController.currentTemperature >= minTemperatureToExit)
            {
                isFrostZoneDefrosted = true;
            }
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
            Debug.Log($"Fading frost effect: {newAlpha}");
            yield return null;
        }

        // Ensure final alpha is set
        Color finalColor = frostOverlay.color;
        finalColor.a = targetAlpha;
        frostOverlay.color = finalColor;
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
}