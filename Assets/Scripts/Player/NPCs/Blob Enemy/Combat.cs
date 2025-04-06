//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class Combat : MonoBehaviour
//{
//    public int damage = 1;

//    private void OnCollisionEnter2D(Collision2D collision)
//    {
//     // Debug.Log("Collision with: " + collision.gameObject.name);

//        PlayerHealth playerHealth = collision.gameObject.GetComponent<PlayerHealth>();

//        if (playerHealth != null)
//        {
//            playerHealth.ChangeHealth(-damage);
//        }
//     /* else
//        {
//            Debug.LogWarning("PlayerHealth component not found on: " + collision.gameObject.name);
//        } */
//    }
//}