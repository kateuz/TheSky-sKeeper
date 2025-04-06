using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    public Animator anim;

    public void Slash()
    {
        anim.SetBool("isAttacking", true);
    }

    public void FinishAttacking()
    {
        anim.SetBool("isAttacking", false);
    }
}
