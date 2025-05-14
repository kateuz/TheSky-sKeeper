using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindCurrent : MonoBehaviour
{
    public float windForce = 10f;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update() {
        


     void OnTriggerStay2D(Collider2D other)
    {
     
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = new Vector2(rb.velocity.x, 0f); 
                rb.AddForce(Vector2.up * windForce, ForceMode2D.Impulse);
            }
        }
    }
}





