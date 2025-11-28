using UnityEngine;
using System.Collections; // Required for Coroutines

public class EnemyAI : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 3f;
    public float health = 30f;
    public float knockbackStrength = 5f; // How hard they get pushed
    
    [Header("References")]
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public Collider2D myCollider;
    public Rigidbody2D rb; // NEW: Physics Component

    private Transform player;
    private bool isDead = false;
    private bool isKnockedBack = false; // Flag to pause movement

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        rb = GetComponent<Rigidbody2D>(); // Auto-find the Rigidbody
    }

    void FixedUpdate() 
    {
        // Physics movement should happen in FixedUpdate
        if (isDead || isKnockedBack || player == null) return;

        // 1. Calculate Direction
        Vector2 direction = (player.position - transform.position).normalized;

        // 2. Move using Physics (Instead of Translate)
        // MovePosition keeps physics collisions working smoothly
        rb.MovePosition(rb.position + direction * speed * Time.fixedDeltaTime);
    }

    // NEW: The Knockback Function
    public void Knockback(Vector2 sourcePosition)
    {
        if (isDead) return;

        // 1. Calculate direction AWAY from the source (Bullet or Player)
        Vector2 pushDir = (transform.position - (Vector3)sourcePosition).normalized;

        // 2. Stop chasing logic temporarily
        isKnockedBack = true;

        // 3. Apply the physical push (Impulse = Instant Force)
        rb.linearVelocity = Vector2.zero; // Reset current speed first
        rb.AddForce(pushDir * knockbackStrength, ForceMode2D.Impulse);

        // 4. Reset after 0.2 seconds so they chase again
        StartCoroutine(ResetKnockback());
    }

    IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(0.2f); // Wait briefly
        rb.linearVelocity = Vector2.zero; // Stop sliding
        isKnockedBack = false; // Resume chasing
    }

    public void TakeDamage(float damageAmount, Vector2 hitSource)
    {
        if (isDead) return;

        health -= damageAmount;
        animator.SetTrigger("Hurt");
        
        // Call Knockback when damaged
        Knockback(hitSource);

        if (health <= 0) Die();
    }

    // NEW: Bounce off the Player if we touch them
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Knock OURSELVES back using the player's position as the source
            Knockback(collision.transform.position);
        }
    }

    void Die()
    {
        isDead = true;
        animator.SetTrigger("Die");
        myCollider.enabled = false;
        rb.linearVelocity = Vector2.zero; // Stop moving completely
        Destroy(gameObject, 1f);
    }
}