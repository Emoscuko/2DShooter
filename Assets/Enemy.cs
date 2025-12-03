using UnityEngine;
using UnityEngine.UI; // Required for Image variables

public class Enemy : MonoBehaviour
{
    [Header("Enemy Stats")]
    public float health = 30f;
    public float speed = 3f;

    [Header("Health Bar UI")]
    public Image healthBarFill; // Drag the Red "Fill" Image here
    public GameObject healthCanvas; // Drag the "HealthCanvas" object here (to hide it on death)

    protected float maxHealth;
    protected Rigidbody2D rb;
    protected Transform player;
    protected bool isDead = false;
    protected Collider2D enemyCollider;

    protected virtual void Start()
    {
        // 1. Remember starting health so we can calculate percentage later
        maxHealth = health;

        rb = GetComponent<Rigidbody2D>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
        
        // 2. Auto-find components if you forgot to drag them
        if (enemyCollider == null) enemyCollider = GetComponent<Collider2D>();
    }

    public virtual void TakeDamage(float amount, Vector2 hitSource)
    {
        if (isDead) return;

        health -= amount;

        // 3. Update the Red Bar
        UpdateHealthBar();

        if (health <= 0) Die();
    }

    // New helper function to update UI
    protected void UpdateHealthBar()
    {
        if (healthBarFill != null)
        {
            // Calculate percentage (e.g., 15 / 30 = 0.5)
            healthBarFill.fillAmount = health / maxHealth;
        }
    }

    protected virtual void Die()
    {
        // Hide the health bar immediately when dead
        if (healthCanvas != null) healthCanvas.SetActive(false);
        Destroy(gameObject);
    }

    public virtual void MoveTowardsPlayer()
    {
        if (player == null || rb == null) return;
        Vector2 direction = (player.position - transform.position).normalized;
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }
}