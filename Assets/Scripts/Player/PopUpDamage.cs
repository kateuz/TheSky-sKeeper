using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpDamage : MonoBehaviour
{
    public Vector2 InitialVelocity;
    public Rigidbody2D rb;
    public float lifeTime = 1.5f;
    public bool hitFromRight;
    void Start()
    {
        if (hitFromRight)
            InitialVelocity.x *= -1;
        rb.velocity = InitialVelocity;
        Destroy(gameObject, lifeTime);
    }

    void Update()
    {
        
    }
}
