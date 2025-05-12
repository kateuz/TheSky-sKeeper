using UnityEngine;

public class FinishPoint : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public QuestManager questManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Killian"))
        { 

            if (GameManager.Instance != null)
            {
                GameManager.Instance.playerHealth = playerHealth.currentHealth;
                GameManager.Instance.oxygenLevel = questManager.oxygenLvl;
            }

            SceneController.instance.NextLevel();
        }
    }
}
