using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    public float damage;
    public float KBForce;

    public Vector2 KBAngle;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        IDamageable damageable = collision.GetComponent<IDamageable>();

        if (damageable != null)
        {
            int facingDirection = transform.position.x > collision.transform.position.x ? -1 : 1;
            damageable.Damage(damage, KBForce, new Vector2 (KBAngle.x * facingDirection, KBAngle.y));
        }
    }
}
