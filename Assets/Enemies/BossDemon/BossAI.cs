using UnityEngine;
using System.Collections;

public class BossAI : Enemy
{
    [Header("References")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    private float attackTimer = 1f;
    private bool isAttacking = false;
    private bool isHurt = false;

    // Helper to get Boss data easily
    private BossStats BossData => stats as BossStats;

    protected override void Start()
    {
        base.Start();

        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (BossData == null) Debug.LogError("Assign BOSS STATS to the Boss object!");
    }

    void FixedUpdate()
    {
        if (player == null || isDead || BossData == null) return;

        if (isAttacking || isHurt)
        {
            rb.linearVelocity = Vector2.zero;
            if (animator != null) animator.SetFloat("Speed", 0f);
            return;
        }

        float distance = Vector2.Distance(transform.position, player.position);

        // Flip
        if (transform.position.x < player.position.x)
            spriteRenderer.flipX = true;
        else
            spriteRenderer.flipX = false;

        // Use Data: attackRange
        if (distance <= BossData.attackRange)
        {
            rb.linearVelocity = Vector2.zero;
            if (animator != null) animator.SetFloat("Speed", 0f);

            if (attackTimer <= 0)
            {
                Attack();
                // Use Data: attackCooldown
                attackTimer = BossData.attackCooldown;
            }
        }
        else
        {
            MoveTowardsPlayer();
            if (animator != null) animator.SetFloat("Speed", 1f);
        }

        if (attackTimer > 0) attackTimer -= Time.fixedDeltaTime;
    }

    void Attack()
    {
        isAttacking = true;
        if (animator != null) animator.SetTrigger("Attack");
        
        // Use Data: attackDelay
        StartCoroutine(DealDamageDelayed(BossData.attackDelay));
    }

    System.Collections.IEnumerator DealDamageDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false;
        if (isDead) yield break;

        float dist = Vector2.Distance(transform.position, player.position);
        
        // Use Data: attackRange
        if (dist <= BossData.attackRange + 1f)
        {
            PlayerHealth playerHP = player.GetComponent<PlayerHealth>();
            if (playerHP != null)
            {
                // Use Data: attackDamage
                playerHP.TakeDamage(BossData.attackDamage);
            }
        }
    }

    public override void TakeDamage(float amount, Vector2 hitSource)
    {
        if (isDead) return;
        base.TakeDamage(amount, hitSource);

        if (!isHurt) StartCoroutine(HurtRoutine());
    }

    IEnumerator HurtRoutine()
    {
        isHurt = true;
        rb.linearVelocity = Vector2.zero;
        if (animator != null) animator.SetTrigger("Hurt");

        // Use Data: hurtDuration
        yield return new WaitForSeconds(BossData.hurtDuration);
        isHurt = false;
    }

    protected override void Die()
    {
        if (isDead) return;
        isDead = true;

        if (animator != null) animator.SetTrigger("Die");
        if (enemyCollider != null) enemyCollider.enabled = false;

        rb.linearVelocity = Vector2.zero;
        this.enabled = false;
        Destroy(gameObject, 2f);
    }
}