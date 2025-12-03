using UnityEngine;
using System.Collections;

public class EnemyAI : Enemy
{
    [Header("Slime Specifics")]
    public float knockbackStrength = 5f;

    [Header("References")]
    public Animator animator;
    public AudioSource audioSource;
    public AudioClip deathSound;

    private bool isKnockedBack = false;

    protected override void Start()
    {
        base.Start(); // Run the Parent's setup (Finding Player + Rigidbody)

        // If these aren't assigned in Inspector, try to find them automatically
        if (animator == null) animator = GetComponent<Animator>();
        if (enemyCollider == null) enemyCollider = GetComponent<Collider2D>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
    }

    void FixedUpdate()
    {
        // Don't move if dead or being knocked back
        if (isDead || isKnockedBack) return;

        MoveTowardsPlayer(); // Use the movement logic from the Parent script
    }

    // OVERRIDE: This runs INSTEAD of the simple "health -=" in the parent
    public override void TakeDamage(float amount, Vector2 hitSource)
    {
        if (isDead) return;

        // 1. Let the Parent script handle the math (Health subtraction)
        base.TakeDamage(amount, hitSource);

        // 2. Play the Hurt animation
        if (animator != null) animator.SetTrigger("Hurt");

        // 3. Do the Slime-specific Knockback
        Knockback(hitSource);
    }

    public void Knockback(Vector2 sourcePosition)
    {
        if (isDead) return;

        Vector2 pushDir = (transform.position - (Vector3)sourcePosition).normalized;
        isKnockedBack = true;

        // Stop current movement so the push force feels clean
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(pushDir * knockbackStrength, ForceMode2D.Impulse);

        StartCoroutine(ResetKnockback());
    }

    IEnumerator ResetKnockback()
    {
        yield return new WaitForSeconds(0.2f);
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerHealth playerHP = collision.gameObject.GetComponent<PlayerHealth>();

            // Deal 1 Damage
            if (playerHP != null)
            {
                playerHP.TakeDamage(1);
            }

            // Bounce back
            Knockback(collision.transform.position);
        }
    }

    // OVERRIDE: Replaces the simple "Destroy" from the parent
    protected override void Die()
    {
        if (isDead) return;

        isDead = true;

        if (animator != null) animator.SetTrigger("Die");
        if (enemyCollider != null) enemyCollider.enabled = false;

        rb.linearVelocity = Vector2.zero; // Stop moving

        if (audioSource != null && deathSound != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        Destroy(gameObject, 1f);
    }
}