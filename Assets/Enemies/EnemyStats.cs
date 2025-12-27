using UnityEngine;

// This adds a menu item to create this asset in the Project window
[CreateAssetMenu(fileName = "NewEnemyStats", menuName = "ScriptableObjects/Enemy Stats/Base Stats")]
public class EnemyStats : ScriptableObject
{
    [Header("Base Stats")]
    public float maxHealth = 30f;
    public float moveSpeed = 3f;
}