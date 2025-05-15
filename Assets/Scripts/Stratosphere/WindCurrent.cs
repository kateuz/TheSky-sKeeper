using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCurrent : MonoBehaviour
{
    public float windForce = 10f;
    public float bounceAmplitude = 2f; // How high the bounce goes
    public float bounceFrequency = 2f; // How fast the bounce happens
    private float originalGravityScale = 4.1f;
    private float timeInWind = 0f;
    private bool isInWind = false;
    public Rigidbody2D playerRb;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Killian"))
        {
            playerRb = other.GetComponent<Rigidbody2D>();
            if (playerRb != null)
            {
                // Store the current gravity scale before changing it
                originalGravityScale = playerRb.gravityScale;
                playerRb.gravityScale = 0f;
                isInWind = true;
                timeInWind = 0f;
                Debug.Log("Entered wind zone. Original gravity: " + originalGravityScale);
            }
        }
    }

    void Update()
    {
        if (isInWind && playerRb != null)
        {
            timeInWind += Time.deltaTime;
            
            // Create bouncing motion
            float bounceOffset = Mathf.Sin(timeInWind * bounceFrequency) * bounceAmplitude;
            
            // Calculate target velocity with bounce
            float targetVelocity = windForce + bounceOffset;
            
            // Apply the new velocity while preserving horizontal movement
            playerRb.velocity = new Vector2(playerRb.velocity.x, targetVelocity);
            
            Debug.Log("In wind zone. Current velocity: " + playerRb.velocity.y);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Killian"))
        {
            if (playerRb != null)
            {
                isInWind = false;
                // Force gravity back to 4.1
                playerRb.gravityScale = 4.1f;
                Debug.Log("Exited wind zone. Restored gravity to: " + playerRb.gravityScale);
                
                // Keep some upward momentum when leaving
                playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y * 0.5f);
            }
        }
    }

    void OnDisable()
    {
        // Ensure gravity is restored if the wind zone is disabled
        if (playerRb != null)
        {
            playerRb.gravityScale = 4.1f;
            Debug.Log("Wind zone disabled. Restored gravity to: " + playerRb.gravityScale);
        }
    }
}





