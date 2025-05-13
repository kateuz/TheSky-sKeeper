using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileEnemy : Enemy
{
    public Transform firePoint;
    public GameObject projectilePrefab;

    public bool CheckForRangedTarget()
    {
        RaycastHit2D hitRangedTarget = Physics2D.Raycast(ledgeDetector.position, facingDirection == 1 ? Vector2.right : Vector2.left, ((ProjectileEnemyStats)stats).rangedAttackDistance, playerLayer);

        if (hitRangedTarget.collider == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
