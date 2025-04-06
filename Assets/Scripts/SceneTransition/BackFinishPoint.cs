using UnityEngine;

public class BackFinishPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Killian"))
        {
            SceneController.instance.NextLevel();
        }
    } 
}
