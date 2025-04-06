using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalSecond : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Killian"))
        {
            SceneManager.LoadScene("P3Troposphere");
        }
    }
}
