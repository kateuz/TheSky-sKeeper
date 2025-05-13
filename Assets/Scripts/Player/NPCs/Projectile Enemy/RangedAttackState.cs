using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedAttackState : EnemyBaseState
{
    private float attackCooldown = 2f;
    private float lastAttackTime;
    private ProjectileEnemy projectileEnemy;

    public RangedAttackState(ProjectileEnemy enemy, string animationName) : base(enemy, animationName)
    {
        projectileEnemy = enemy;
    }

    public override void Enter()
    {
        base.Enter();
        lastAttackTime = Time.time;
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (!projectileEnemy.CheckForRangedTarget())
        {
            if (projectileEnemy.CheckForPlayer())
            {
                projectileEnemy.SwitchState(projectileEnemy.playerDetectedState);
            }
            else
            {
                projectileEnemy.SwitchState(projectileEnemy.patrolState);
            }
        }
        else if (Time.time >= lastAttackTime + attackCooldown)
        {
            Attack();
            lastAttackTime = Time.time;
        }
    }

    private void Attack()
    {
        // Spawn projectile
        GameObject projectile = Object.Instantiate(projectileEnemy.projectilePrefab, projectileEnemy.firePoint.position, projectileEnemy.firePoint.rotation);
        Rigidbody2D projectileRb = projectile.GetComponent<Rigidbody2D>();
        
        if (projectileRb != null)
        {
            // Calculate direction to player
            Vector2 direction = (GameObject.FindGameObjectWithTag("Player").transform.position - projectileEnemy.transform.position).normalized;
            ProjectileEnemyStats stats = (ProjectileEnemyStats)projectileEnemy.stats;
            projectileRb.velocity = direction * stats.projectileSpeed;
        }
    }
}