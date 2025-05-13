using UnityEngine;
using System.Collections;

public class MeteorManager : MonoBehaviour
{
    [Header("Meteor Settings")]
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private float spawnHeight = 10f; // Fixed spawn height
    [SerializeField] private float meteorSpeed = 5f;
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private GameObject hitEffectPrefab;

    private Transform playerTransform;
    private MesospherePlayerMovement playerMovement;
    private Camera mainCamera;

    private void Start()
    {
        // Find the player
        GameObject player = GameObject.FindGameObjectWithTag("Killian");
        if (player != null)
        {
            playerTransform = player.transform;
            playerMovement = player.GetComponent<MesospherePlayerMovement>();
        }
        else
        {
            Debug.LogError("Player not found! Make sure the player has the 'Killian' tag.");
        }

        // Get the main camera
        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
        }

        StartCoroutine(SpawnMeteors());
    }

    private IEnumerator SpawnMeteors()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (playerTransform == null || mainCamera == null) continue;

            // Spawn position at the top center of the camera view
            Vector3 spawnPosition = mainCamera.transform.position + new Vector3(0f, spawnHeight, 0f);

            // Spawn meteor
            GameObject meteor = Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
            meteor.transform.SetParent(mainCamera.transform, true); // Make it a child of the camera
            
            // Calculate direction to player
            Vector2 directionToPlayer = (playerTransform.position - spawnPosition).normalized;
            
            // Add velocity towards player
            Rigidbody2D rb = meteor.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = directionToPlayer * meteorSpeed;
            }

            // Ensure meteor is fully opaque
            SpriteRenderer sr = meteor.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                Color color = sr.color;
                color.a = 1f;
                sr.color = color;
            }

            // Add damage component
            MeteorDamage meteorDamage = meteor.AddComponent<MeteorDamage>();
            meteorDamage.damageAmount = damageAmount;
            meteorDamage.hitEffectPrefab = hitEffectPrefab;

            // Destroy after some time if it doesn't hit anything
            Destroy(meteor, 10f);
        }
    }
}

public class MeteorDamage : MonoBehaviour
{
    public float damageAmount;
    public GameObject hitEffectPrefab;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Killian"))
        {
            PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();
            MesospherePlayerMovement playerMovement = collision.gameObject.GetComponent<MesospherePlayerMovement>();
            
            // Only damage if player doesn't have shield
            if (playerHealth != null && (playerMovement == null || !playerMovement.isShieldActive))
            {
                playerHealth.Damage(damageAmount);
                
                // Spawn hit effect
                if (hitEffectPrefab != null)
                {
                    Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
                }
            }
        }
        Destroy(gameObject);
    }
} 