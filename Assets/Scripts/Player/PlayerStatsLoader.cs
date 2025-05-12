using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatsLoader : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public QuestManager questManager;

    void Start()
    {
        if (GameManager.Instance != null)
        {
            playerHealth.currentHealth = GameManager.Instance.playerHealth;
            questManager.oxygenLvl = GameManager.Instance.oxygenLevel;
        }
    }
}
