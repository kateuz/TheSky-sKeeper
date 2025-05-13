using UnityEngine;

[CreateAssetMenu(menuName = "ProjectileEnemyStats")]
public class ProjectileEnemyStats : StatsSO
{
    [Header("Ranged Attack")]
    public float rangedAttackDistance = 5f;
    public float projectileSpeed = 5f;
    public float projectileDamage = 10f;
}