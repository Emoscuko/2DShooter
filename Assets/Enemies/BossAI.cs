using UnityEngine;
using System.Collections;

public class BossAI : Enemy
{
    [Header("Boss Specifics")]
    public float attackRange = 2f;
    public float attackDelay = 1.2f;    // Time before damage happens
    public float attackCooldown = 1.2f; // Time between attacks
    public int attackDamage = 2;
    public float hurtDuration = 0.3f; // How long to freeze when hit


    [Header("References")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private float attackTimer = 1f;

    // --- NEW FLAGS ---
    private bool isAttacking = false;
    private bool isHurt = false;

    protected override void Start()
    {
        base.Start();

        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        if (enemyCollider == null) enemyCollider = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        if (player == null || isDead) return;

        // 1. MOVEMENT LOCK (Attacking or Hurt)
        if (isAttacking || isHurt)
        {
            rb.linearVelocity = Vector2.zero;
            if (animator != null) animator.SetFloat("Speed", 0f); // Force Idle
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        // 2. FLIP LOGIC
        if (transform.position.x < player.position.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;

        // 3. DECISION LOGIC
        if (distance <= attackRange)
        {
            // --- STOPPED (In Range) ---
            rb.linearVelocity = Vector2.zero;

            // Tell animator we are STOPPED
            if (animator != null) animator.SetFloat("Speed", 0f);

            if (attackTimer <= 0)
            {
                Attack();
                attackTimer = attackCooldown;
            }
        }
        else
        {
            // --- MOVING (Chasing) ---
            MoveTowardsPlayer();

            // Tell animator we are MOVING
            // We set it to 1f (which is > 0.01f), so it will transition to Walk
            if (animator != null) animator.SetFloat("Speed", 1f);
        }

        if (attackTimer > 0) attackTimer -= Time.fixedDeltaTime;
    }

    void Attack()
    {
        // 1. Lock movement
        isAttacking = true;

        if (animator != null) animator.SetTrigger("Attack");

        // 2. Start the delayed damage
        StartCoroutine(DealDamageDelayed(attackDelay));
    }

    System.Collections.IEnumerator DealDamageDelayed(float delay)
    {
        // Wait for the swing
        yield return new WaitForSeconds(delay);

        // --- NEW: Unlock movement now that attack is finished ---
        isAttacking = false;

        if (isDead) yield break;

        float dist = Vector2.Distance(transform.position, player.position);

        if (dist <= attackRange + 1f)
        {
            PlayerHealth playerHP = player.GetComponent<PlayerHealth>();
            if (playerHP != null)
            {
                playerHP.TakeDamage(attackDamage);
            }
        }
    }

    public override void TakeDamage(float amount, Vector2 hitSource)
    {
        if (isDead) return;

        base.TakeDamage(amount, hitSource);

        // --- NEW: Handle the Hurt State ---
        if (!isHurt) // Only trigger if not already hurt (prevents stun-lock glitches)
        {
            StartCoroutine(HurtRoutine());
        }
    }

    // New Coroutine to freeze boss while hurting
    IEnumerator HurtRoutine()
    {
        isHurt = true;
        rb.linearVelocity = Vector2.zero; // Stop instantly

        if (animator != null) animator.SetTrigger("Hurt");

        // Wait for the animation to finish (approx 0.5s)
        yield return new WaitForSeconds(hurtDuration);

        isHurt = false; // Allow moving again
    }

    protected override void Die()
    {
        if (isDead) return;
        isDead = true;

        // CHANGE: Pass the variable here too. 
        // Note: You could even hardcode a bonus here if you wanted, 
        // e.g., GrantXP(xpReward * 2);
        GrantXP(100f);
        // ---------------------------------------------------------------------------

        if (animator != null) animator.SetTrigger("Die");
        // ... rest of your code
    }
}