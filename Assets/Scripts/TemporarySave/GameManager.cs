using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public string savedWeather;
    public float oxygenLevel = 100f;
    public float playerHealth = 100f;
    public float savedTemp = 25f;
    public bool hasWand = false;
    public bool hasScanner = false;
    public bool isRespawning = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame();
            Debug.Log("New GameManager instance created");
        }
        else
        {
            Debug.Log("GameManager already exists, destroying duplicate");
            Destroy(gameObject); // prevent duplicates
        }
    }

    private void InitializeGame()
    {
        // Initialize health if it's the first time
        if (playerHealth <= 0)
        {
            playerHealth = 100f;
        }
        
        Debug.Log($"GameManager initialized with health: {playerHealth}");
    }

    public void SavePlayerHealth(float health)
    {
        playerHealth = health;
        Debug.Log($"Player health saved: {playerHealth}");
    }

    // Call this when transitioning to a new scene
    public void OnSceneLoaded()
    {
        Debug.Log($"GameManager in new scene. Current health: {playerHealth}");
    }
}

