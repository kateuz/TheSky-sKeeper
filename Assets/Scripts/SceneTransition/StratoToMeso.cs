using UnityEngine;
using UnityEngine.SceneManagement;

public class StratoToMeso : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Killian"))
        {
            SceneManager.LoadScene("Mesosphere");
        }
    }
}
