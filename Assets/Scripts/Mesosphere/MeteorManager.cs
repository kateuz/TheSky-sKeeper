using UnityEngine;
using System.Collections;

public class MeteorManager : MonoBehaviour
{
    [Header("Meteor Settings")]
    [SerializeField] private GameObject meteorPrefab;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private float minSpawnHeight = 10f;
    [SerializeField] private float maxSpawnHeight = 15f;
    [SerializeField] private float meteorSpeed = 5f;
    [SerializeField] private float damageAmount = 10f;
    [SerializeField] private GameObject hitEffectPrefab;

    [Header("Spawn Area")]
    [SerializeField] private float minX = -10f;
    [SerializeField] private float maxX = 10f;

    private Transform playerTransform;
    private MesospherePlayerMovement playerMovement;

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

        StartCoroutine(SpawnMeteors());
    }

    private IEnumerator SpawnMeteors()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (playerTransform == null) continue;

            // Random spawn position
            float randomX = Random.Range(minX, maxX);
            float spawnHeight = Random.Range(minSpawnHeight, maxSpawnHeight);
            Vector3 spawnPosition = new Vector3(randomX, spawnHeight, 0f);

            // Spawn meteor
            GameObject meteor = Instantiate(meteorPrefab, spawnPosition, Quaternion.identity);
            
            // Add velocity
            Rigidbody2D rb = meteor.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = Vector2.down * meteorSpeed;
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