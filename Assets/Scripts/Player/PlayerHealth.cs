using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float currentHealth;
    public float maxHealth;
    public Slider slider;
    public Animator hitParticleAnim;
    public GameObject gameOverImg;

    void Start()
    {
        slider.maxValue = maxHealth;
        slider.value = currentHealth;

        if (gameOverImg != null )
        {
            gameOverImg.SetActive(false);
        }
    }

    void Update()
    {
        if (transform.position.y <= -9f)
        {
            Die();
        }
    }

    public void Damage(float damageAmount) // Changed to int
    {
        if (hitParticleAnim != null) // Null check
        {
            hitParticleAnim.Play("HitParticle");
        }

        currentHealth -= damageAmount;
        slider.value = currentHealth; // Update slider

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (gameOverImg != null)
        {
            gameOverImg.SetActive(true);
        }
        gameObject.SetActive(false);
    }

    // Example healing method
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth);
        slider.value = currentHealth;
    }

    public void Damage(float damageAmount, float KBForce, Vector2 KBAngle) { }
}