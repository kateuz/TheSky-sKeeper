using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack2 : MonoBehaviour
{
    private bool attacking = false;

    private float attackTimer = 0;
    private float attackCd = 0.3f;

    public Collider2D attackTrigger;

    private Animator anim;

    void Awake()
    {
        anim = gameObject.GetComponent<Animator>();
        attackTrigger.enabled = false;
    }

    void Update()
    {
        //    if (Input.GetKeyDown("k") && !attacking)
        //    {
        //        attacking = true;
        //        attackTimer = attackCd;

        //        attackTrigger.enabled = true;
        //    }

        //    if (attacking)
        //    {
        //        if (attackTimer > 0)
        //        {
        //            attackTimer -= Time.deltaTime;
        //        }
        //        else
        //        {
        //            attacking = false;
        //            attackTrigger.enabled = false;
        //        }
        //    }

        //    anim.SetBool("isAttacking", attacking);
        //}
    }

    public void Attack()
    {
        if (!attacking)
        {
            attacking = true;
            attackTimer = attackCd;

            attackTrigger.enabled = true;
        }

        if (attacking)
        {
            if (attackTimer > 0)
            {
                attackTimer -= Time.deltaTime;
            }
            else
            {
                attacking = false;
                attackTrigger.enabled = false;
            }
        }

        anim.SetBool("isAttacking", attacking);
    }
}
