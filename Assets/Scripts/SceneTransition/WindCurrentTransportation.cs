using UnityEngine;
using UnityEngine.SceneManagement;

public class WindCurrentTransportation : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Killian"))
        {
            SceneManager.LoadScene("Mesosphere"); //StratospherePt2
        } 
    }
}
