using UnityEngine;

[CreateAssetMenu(fileName = "NewSlimeStats", menuName = "ScriptableObjects/Enemy Stats/Slime Stats")]
public class SlimeStats : EnemyStats // Inherits from EnemyStats
{
    [Header("Slime Specifics")]
    public float knockbackStrength = 5f;
    public AudioClip deathSound;
}