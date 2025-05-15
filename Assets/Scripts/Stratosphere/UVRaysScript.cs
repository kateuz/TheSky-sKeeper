using UnityEngine;
using System.Collections;

public class UVRaysScript : MonoBehaviour
{
    [Header("UV Ray Settings")]
    [SerializeField] private GameObject sunRayPrefab;
    [SerializeField] private float spawnInterval = 5f;
    [SerializeField] private float spawnHeight = 10f;
    [SerializeField] private float raySpeed = 5f;
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

        StartCoroutine(SpawnSunRays());
    }

    private IEnumerator SpawnSunRays()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (playerTransform == null || mainCamera == null) continue;

            // Spawn position at the top center of the camera view
            Vector3 spawnPosition = mainCamera.transform.position + new Vector3(0f, spawnHeight, 0f);

            // Spawn sun ray
            GameObject sunRay = Instantiate(sunRayPrefab, spawnPosition, Quaternion.identity);
            sunRay.transform.SetParent(mainCamera.transform, true);

            // Calculate direction to player
            Vector2 directionToPlayer = (playerTransform.position - spawnPosition).normalized;

            // Add velocity towards player
            Rigidbody2D rb = sunRay.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = directionToPlayer * raySpeed;
            }

            // Add damage component
            UVRayDamage uvRayDamage = sunRay.AddComponent<UVRayDamage>();
            uvRayDamage.damageAmount = damageAmount;
            uvRayDamage.hitEffectPrefab = hitEffectPrefab;

            // Destroy after some time if it doesn't hit anything
            Destroy(sunRay, 10f);
        }
    }
}

public class UVRayDamage : MonoBehaviour
{
    public float damageAmount;
    public GameObject hitEffectPrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log($"UV Ray hit something: {collision.gameObject.name}");
        
        if (collision.gameObject.CompareTag("Killian"))
        {
            Debug.Log("UV Ray hit player!");
            StratospherePlayerHealth playerHealth = collision.gameObject.GetComponent<StratospherePlayerHealth>();
            MesospherePlayerMovement playerMovement = collision.gameObject.GetComponent<MesospherePlayerMovement>();

            // Only damage if player doesn't have shield
            if (playerHealth != null && (playerMovement == null || !playerMovement.isShieldActive))
            {
                Debug.Log("Applying UV damage to player!");
                playerHealth.Damage(damageAmount);

                // Spawn hit effect
                if (hitEffectPrefab != null)
                {
                    Instantiate(hitEffectPrefab, transform.position, Quaternion.identity);
                }
            }
            else
            {
                Debug.Log("Player has shield active or health component missing");
            }
        }
        Destroy(gameObject);
    }
}