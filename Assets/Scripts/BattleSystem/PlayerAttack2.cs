using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack2 : MonoBehaviour
{
    private bool isAttacking = false;

    private float attackTimer = 0;
    private float attackCd = 0.3f;

    public Collider2D AttackTrigger;

    private Animator anim;

    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        AttackTrigger.enabled = false;
    }

    void Update()
    {
        if (Input.GetKeyDown("k") && !isAttacking) 
        {
            isAttacking = true;
            attackTimer = attackCd;

            AttackTrigger.enabled = true;
        }
        if (isAttacking)
        {
            if (attackTimer > 0) 
            { 
                attackTimer -= Time.deltaTime;
            } else
            {
                isAttacking = false;
                AttackTrigger.enabled = false;
            }
        }

        anim.SetBool("isAttacking", isAttacking);
    }
}
