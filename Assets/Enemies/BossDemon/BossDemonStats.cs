using UnityEngine;

[CreateAssetMenu(fileName = "NewBossStats", menuName = "ScriptableObjects/Enemy Stats/Boss Stats")]
public class BossStats : EnemyStats // Inherits from EnemyStats
{
    [Header("Combat")]
    public float attackRange = 2f;
    public int attackDamage = 2;
    
    [Header("Timing")]
    public float attackDelay = 1.2f;    // Time before damage happens
    public float attackCooldown = 1.2f; // Time between attacks
    public float hurtDuration = 0.3f;   // Stun time
}