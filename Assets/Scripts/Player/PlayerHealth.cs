using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour, IDamageable
{
    public float currentHealth;
    public float maxHealth;
    public Slider slider;
    public Animator hitParticleAnim;

    void Start()
    {
        slider.maxValue = maxHealth;
        slider.value = currentHealth;
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
            Destroy(gameObject);
        }
    }

    // Example healing method
    public void Heal(int healAmount)
    {
        currentHealth += healAmount;
        currentHealth = Mathf.Clamp(currentHealth, 0, maxHealth); // Clamp to max
        slider.value = currentHealth; // Update slider
    }

    public void Damage(float damageAmount, float KBForce, Vector2 KBAngle) { }
}