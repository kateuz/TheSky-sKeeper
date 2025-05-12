using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float currentHealth;
    public float maxHealth = 100f;

    public bool isProtected = false; //lightning rod shield
    public bool isMeteorShieldActive = false; //meteor shield

    public Slider slider;
    public Animator hitParticleAnim;
    public GameObject gameOverImg;

    private SpriteRenderer sr;

    void Start()
    {
        // Initialize health from GameManager or default
        if (GameManager.Instance != null)
        {
            if (GameManager.Instance.isRespawning)
            {
                currentHealth = maxHealth;
                GameManager.Instance.playerHealth = maxHealth;
                GameManager.Instance.isRespawning = false;
                Debug.Log("Player respawned with full health");
            }
            else
            {
                currentHealth = GameManager.Instance.playerHealth;
                Debug.Log($"Loaded health from GameManager: {currentHealth}");
            }
        }
        else
        {
            currentHealth = maxHealth;
            Debug.Log("GameManager not found, using default health");
        }

        // Ensure slider is properly set up
        if (slider != null)
        {
            slider.maxValue = maxHealth;
            slider.value = currentHealth;
            Debug.Log($"Health slider initialized: {currentHealth}/{maxHealth}");
        }
        else
        {
            Debug.LogError("Health Slider is not assigned in the inspector!");
        }

        // Ensure game over image is properly set up
        if (gameOverImg != null)
        {
            gameOverImg.SetActive(false);
            Debug.Log("Game Over Image is ready");
        }
        else
        {
            Debug.LogError("Game Over Image is not assigned in the inspector!");
        }

        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (transform.position.y <= -9f)
        {
            Die();
        }
    }

    public void Damage(float damageAmount)
    {
        // Check if player is protected by either shield
        if (isProtected || isMeteorShieldActive)
        {
            Debug.Log("Player is protected from damage");
            return;
        }

        if (hitParticleAnim != null)
        {
            hitParticleAnim.Play("Hit");
        }

        StartCoroutine(FlashRed());

        currentHealth -= damageAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        Debug.Log($"Player took {damageAmount} damage. Current health: {currentHealth}");

        if (slider != null)
        {
            slider.value = currentHealth; //slider is synced
            Debug.Log($"Slider value updated to: {slider.value}");
        }
        else
        {
            Debug.LogError("Cannot update health slider - reference is null!");
        }
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SavePlayerHealth(currentHealth);
        }

        if (currentHealth <= 0)
        {
            Debug.Log("Player health reached 0, calling Die()");
            Die();
        }
    }

    void Die()
    {
        Debug.Log("Die() called");
        
        if (gameOverImg != null)
        {
            gameOverImg.SetActive(true);
            Debug.Log("Game Over Image activated");
        }
        else
        {
            Debug.LogError("Cannot show game over - image reference is null!");
        }

        if (GameManager.Instance != null)
        {
            GameManager.Instance.isRespawning = true;
        }
        
        // Add a small delay before deactivating the player
        StartCoroutine(DeactivatePlayerWithDelay());
    }

    private IEnumerator DeactivatePlayerWithDelay()
    {
        yield return new WaitForSeconds(0.5f); // Give time for the game over screen to be visible
        gameObject.SetActive(false);
    }

    // Example healing method
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        if (slider != null)
        {
            slider.value = currentHealth;
            Debug.Log($"Player healed for {healAmount}. Current health: {currentHealth}");
        }
        
        if (GameManager.Instance != null)
        {
            GameManager.Instance.SavePlayerHealth(currentHealth);
        }
    }

    public void Damage(float damageAmount, float KBForce, Vector2 KBAngle) { }

    private IEnumerator FlashRed()
    {
        if (sr != null)
        {
            sr.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            sr.color = Color.white;
        }
    }
}