using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagedState : EnemyBaseState
{

    public float KBForce;
    public Vector2 KBAngle;

   // private float stunTime = 1;
    public DamagedState(Enemy enemy, string animationName) : base(enemy, animationName)
    {

    }

    public override void Enter()
    {
        base.Enter();

        ApplyKnockback();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        int forceDirection = enemy.rb.velocity.x > 0 ? -1 : 1;

        if (enemy.facingDirection != forceDirection) {
            enemy.Rotate();
        }
    }

    public override void PhysicsUpdate()
    {
        base.PhysicsUpdate();
    }

    private void ApplyKnockback()
    {
        enemy.rb.velocity = KBAngle * KBForce;
    }
}
