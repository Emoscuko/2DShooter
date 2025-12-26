using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [Header("Data Asset")]
    public EnemyStats stats; // DRAG YOUR STATS ASSET HERE

    [Header("Health Bar UI")]
    public Image healthBarFill; 
    public GameObject healthCanvas;

    // We keep currentHealth here because it changes during gameplay
    protected float currentHealth;
    
    protected Rigidbody2D rb;
    protected Transform player;
    protected bool isDead = false;
    protected Collider2D enemyCollider;

    protected virtual void Start()
    {
        if (stats == null)
        {
            Debug.LogError("NO STATS ASSIGNED to " + gameObject.name);
            return;
        }

        // Initialize health from the ScriptableObject
        currentHealth = stats.maxHealth;

        rb = GetComponent<Rigidbody2D>();
        
        // Auto-find components
        if (enemyCollider == null) enemyCollider = GetComponent<Collider2D>();

        GameObject p = GameObject.FindGameObjectWithTag("Player");
        if (p != null) player = p.transform;
    }

    public virtual void TakeDamage(float amount, Vector2 hitSource)
    {
        if (isDead) return;

        currentHealth -= amount;
        UpdateHealthBar();

        if (currentHealth <= 0) Die();
    }

    protected void UpdateHealthBar()
    {
        if (healthBarFill != null && stats != null)
        {
            // Use stats.maxHealth for the calculation
            healthBarFill.fillAmount = currentHealth / stats.maxHealth;
        }
    }

    protected virtual void Die()
    {
        if (healthCanvas != null) healthCanvas.SetActive(false);
        Destroy(gameObject);
    }

    public virtual void MoveTowardsPlayer()
    {
        if (player == null || rb == null || stats == null) return;
        
        Vector2 direction = (player.position - transform.position).normalized;
        // Use stats.moveSpeed
        rb.MovePosition(rb.position + direction * stats.moveSpeed * Time.fixedDeltaTime);
    }
}