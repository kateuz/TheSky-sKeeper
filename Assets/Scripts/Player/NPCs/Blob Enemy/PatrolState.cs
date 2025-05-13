using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
//using static UnityEngine.RuleTile.TilingRuleOutput;

public class PatrolState : EnemyBaseState
{
    public PatrolState(Enemy enemy, string animationName) : base(enemy, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (enemy.CheckForPlayer())
            enemy.SwitchState(enemy.playerDetectedState);
        if (enemy.CheckForObstacles())
            enemy.Rotate();
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();

        if (enemy.facingDirection == 1)
        {
            enemy.rb.velocity = new Vector2(enemy.stats.speed, enemy.rb.velocity.y);
        }
        else
        {
            enemy.rb.velocity = new Vector2(-enemy.stats.speed, enemy.rb.velocity.y);
        }
    }
}