using UnityEngine;

public class DamagedState : EnemyBaseState
{
    public float KBForce;
    public Vector2 KBAngle;
    private float stunDuration = 0.3f;
    private float stunStartTime;

    public DamagedState(Enemy enemy, string animationName) : base(enemy, animationName) { }

    public override void Enter()
    {
        base.Enter();
        stunStartTime = Time.time;
        ApplyKnockback();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();

        if (Time.time >= stunStartTime + stunDuration)
        {
            if (enemy.CheckForPlayer())
            {
                enemy.SwitchState(enemy.playerDetectedState);
            }
            else
            {
                enemy.SwitchState(enemy.patrolState);
            }
        }
    }

    private void ApplyKnockback()
    {
        enemy.rb.velocity = new Vector2(KBAngle.x * enemy.facingDirection, KBAngle.y) * KBForce;
    }
}
