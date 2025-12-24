using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float health = 30f;
    public float speed = 3f;

    // --- NEW: Add this variable ---
    [Header("Rewards")]
    public int xp = 20; // Default value, change this in Inspector for Bosses
                        // -----------------------------

    [Header("UI Template")]
    // Drag your Health Bar Prefab into this slot in the Inspector
    public GameObject healthBarPrefab;
    public float healthBarHeight = 1.5f;

    // These will be assigned automatically when the game starts
    protected Image healthBarFill;
    protected GameObject spawnedCanvas;

    protected float maxHealth;
    protected Rigidbody2D rb;
    protected Transform player;
    protected bool isDead = false;
    protected Collider2D enemyCollider;

    protected virtual void Start()
    {
        maxHealth = health;
        rb = GetComponent<Rigidbody2D>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;

        if (enemyCollider == null) enemyCollider = GetComponent<Collider2D>();

        // --- FIX STARTS HERE ---
        if (healthBarPrefab != null)
        {
            // 1. Instantiate (Spawn) the health bar and make it a child of the Enemy
            spawnedCanvas = Instantiate(healthBarPrefab, transform.position, Quaternion.identity, transform);

            // Optional: Move it up slightly so it doesn't overlap the enemy sprite
            spawnedCanvas.transform.localPosition = new Vector3(0, healthBarHeight, 0);

            // 2. NOW look for the fill image inside the spawned object
            // Ensure your prefab hierarchy matches this path exactly:
            Transform fillTransform = spawnedCanvas.transform.Find("Background/Fill");

            if (fillTransform != null)
            {
                healthBarFill = fillTransform.GetComponent<Image>();
            }
            else
            {
                Debug.LogError("Could not find 'Background/Fill' inside the HealthBar prefab!");
            }
        }
        // --- FIX ENDS HERE ---
    }

    public virtual void TakeDamage(float amount, Vector2 hitSource)
    {
        if (isDead) return;

        health -= amount;
        Debug.Log(gameObject.name + " took damage! Current health: " + health);

        UpdateHealthBar();
        if (health <= 0) Die();
    }

    protected void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            healthBarFill.fillAmount = health / maxHealth;
        }
    }

    // --- NEW: Helper function to give XP ---
    protected void GrantXP(float xpReward)
    {
        if (player != null)
        {
            // Try to find the new script on the player
            PlayerExperience playerXP = player.GetComponent<PlayerExperience>();
            if (playerXP != null)
            {
                playerXP.GainExperience(xpReward);
            }
        }
    }
    // --------------------------------------

    protected virtual void Die()
    {
        if (isDead) return;
        isDead = true;

        GrantXP(xp);

        // No need to manually deactivate; it will be destroyed with the enemy
        Destroy(gameObject);
    }

    public virtual void MoveTowardsPlayer()
    {
        if (player == null || rb == null) return;
        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }
}