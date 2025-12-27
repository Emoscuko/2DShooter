using UnityEngine;

[CreateAssetMenu(fileName = "NewBossStats", menuName = "ScriptableObjects/Enemy Stats/Boss Stats")]
public class BossStats : EnemyStats
{
    [Header("Combat")]
    public float attackRange = 2f;
    public int attackDamage = 2;

    [Header("Timing")]
    public float attackDelay = 1.2f;    // Time before damage happens
    public float attackCooldown = 1.2f; // Time between attacks
    public float hurtDuration = 0.3f;   // Stun time

    [Header("Shield System")]
    public float maxShield = 10f;       // Health of the shield
    public float shieldRegenDelay = 3f; // Time before shield comes back

    [Header("Rage Mode")]
    public float rageThreshold = 0.5f;       // 0.5 = 50% Health
    public float rageSpeedMultiplier = 1.5f; // 50% faster movement
    public float rageCooldownMultiplier = 0.5f; // Attacks 2x faster (half cooldown)
}