using UnityEngine;
using System.Collections;

public class EnemyAI : Enemy
{
    [Header("References")]
    public Animator animator;
    public AudioSource audioSource;

    private bool isKnockedBack = false;
    
    // Helper property to get the specific stats easily
    private SlimeStats SlimeData => stats as SlimeStats;

    protected override void Start()
    {
        base.Start(); // Run Parent setup

        if (animator == null) animator = GetComponent<Animator>();
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        
        // Safety check
        if (SlimeData == null) Debug.LogWarning("Assigned Stats are NOT SlimeStats on " + gameObject.name);
    }

    void FixedUpdate()
    {
        if (isDead || isKnockedBack) return;
        MoveTowardsPlayer(); 
    }

    public override void TakeDamage(float amount, Vector2 hitSource)
    {
        if (isDead) return;

        base.TakeDamage(amount, hitSource);

        if (animator != null) animator.SetTrigger("Hurt");
        Knockback(hitSource);
    }

    public void Knockback(Vector2 sourcePosition)
    {
        if (isDead || SlimeData == null) return;

        Vector2 pushDir = (transform.position - (Vector3)sourcePosition).normalized;
        isKnockedBack = true;

        rb.linearVelocity = Vector2.zero;
        // Access knockbackStrength from the Data File
        rb.AddForce(pushDir * SlimeData.knockbackStrength, ForceMode2D.Impulse);

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
            if (playerHP != null) playerHP.TakeDamage(1);
            
            Knockback(collision.transform.position);
        }
    }

    protected override void Die()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null) animator.SetTrigger("Die");
        if (enemyCollider != null) enemyCollider.enabled = false;

        rb.linearVelocity = Vector2.zero;

        // Access deathSound from the Data File
        if (audioSource != null && SlimeData != null && SlimeData.deathSound != null)
        {
            audioSource.PlayOneShot(SlimeData.deathSound);
        }

        Destroy(gameObject, 1f);
    }
}