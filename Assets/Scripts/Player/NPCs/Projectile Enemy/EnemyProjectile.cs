using UnityEngine;

public class EnemyProjectile : MonoBehaviour
{
    [SerializeField] private float damage = 10f;
    [SerializeField] private float lifetime = 3f;
    [SerializeField] private GameObject hitEffect;

    private void Start()
    {
        // Destroy projectile after lifetime
        Destroy(gameObject, lifetime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
 
        if (collision.CompareTag("Killian"))
        {
            // Apply damage to player
            PlayerHealth playerHealth = collision.GetComponent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.Damage(damage);
            }

      
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }

            Destroy(gameObject);
        }
        else if (collision.CompareTag("Ground"))
        {
            // Spawn hit effect
            if (hitEffect != null)
            {
                Instantiate(hitEffect, transform.position, Quaternion.identity);
            }
            Destroy(gameObject);
        }
    }
}