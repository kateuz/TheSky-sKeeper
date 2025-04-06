using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathState : EnemyBaseState
{
    public DeathState(Enemy enemy, string animationName) : base(enemy, animationName)
    {

    }
    public override void Enter()
    {
        base.Enter();

        DeathParticles();
        DropItems();

        enemy.gameObject.SetActive(false);

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void DeathParticles()
    {
        enemy.Instantiate(enemy.stats.deathParticle, 1, 0);

        foreach (var debris in enemy.stats.deathDebris)
        {
            enemy.Instantiate(debris, enemy.dropForce, 5);
        }
    }

    private void DropItems()
    {
        foreach (var itemDrop in enemy.itemDrops)
        {
            enemy.Instantiate(itemDrop, enemy.dropForce, enemy.torque);
        }
    }
}
