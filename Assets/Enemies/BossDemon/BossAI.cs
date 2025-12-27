using UnityEngine;
using System.Collections;

public class BossAI : Enemy
{
    [Header("References")]
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public GameObject shieldVisuals; // <--- Drag your 'ShieldVisuals' object here in Inspector

    // --- Runtime State ---
    private float attackTimer = 1f;
    private bool isAttacking = false;
    private bool isHurt = false;
    
    // Shield & Rage Flags
    private float currentShield;
    private bool isShieldBroken = false;
    private bool isRaged = false;

    // Helper to get Boss data easily
    private BossStats BossData => stats as BossStats;

    protected override void Start()
    {
        base.Start();

        if (animator == null) animator = GetComponent<Animator>();
        if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
        
        if (BossData == null)
        {
            Debug.LogError("Assign BOSS STATS to the Boss object!");
            return;
        }

        // Initialize Shield
        currentShield = BossData.maxShield;
        isShieldBroken = false;
        
        // Ensure shield is visible at start
        if(shieldVisuals != null) shieldVisuals.SetActive(true);
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

        // Combat Logic
        if (distance <= BossData.attackRange)
        {
            rb.linearVelocity = Vector2.zero;
            if (animator != null) animator.SetFloat("Speed", 0f);

            if (attackTimer <= 0)
            {
                Attack();
                
                float cooldown = BossData.attackCooldown;
                if (isRaged) cooldown *= BossData.rageCooldownMultiplier;

                attackTimer = cooldown;
            }
        }
        else
        {
            MoveWithRage(); 
            if (animator != null) animator.SetFloat("Speed", 1f);
        }

        if (attackTimer > 0) attackTimer -= Time.fixedDeltaTime;
    }

    void MoveWithRage()
    {
        if (player == null) return;

        Vector2 direction = (player.position - transform.position).normalized;
        float finalSpeed = BossData.moveSpeed;

        if (isRaged) finalSpeed *= BossData.rageSpeedMultiplier;

        rb.MovePosition(rb.position + direction * finalSpeed * Time.fixedDeltaTime);
    }

    void Attack()
    {
        isAttacking = true;
        if (animator != null) animator.SetTrigger("Attack");
        
        float delay = BossData.attackDelay;
        if (isRaged) delay *= BossData.rageCooldownMultiplier; 

        StartCoroutine(DealDamageDelayed(delay));
    }

    System.Collections.IEnumerator DealDamageDelayed(float delay)
    {
        yield return new WaitForSeconds(delay);
        isAttacking = false; 
        if (isDead) yield break;

        float dist = Vector2.Distance(transform.position, player.position);
        if (dist <= BossData.attackRange + 1f)
        {
            PlayerHealth playerHP = player.GetComponent<PlayerHealth>();
            if (playerHP != null)
            {
                playerHP.TakeDamage(BossData.attackDamage);
            }
        }
    }

    // --- DAMAGE & SHIELD LOGIC ---
    public override void TakeDamage(float amount, Vector2 hitSource)
    {
        if (isDead) return;

        // 1. SHIELD CHECK
        if (!isShieldBroken && currentShield > 0)
        {
            currentShield -= amount;
            
            // Optional: Flash the shield visual slightly to show impact
            StartCoroutine(FlashShieldColor()); 

            if (currentShield <= 0)
            {
                BreakShield();
            }
            
            // Prevent health damage
            return;
        }

        // 2. HEALTH DAMAGE
        base.TakeDamage(amount, hitSource); 

        // 3. RAGE CHECK
        float healthPercent = currentHealth / BossData.maxHealth;
        if (!isRaged && healthPercent <= BossData.rageThreshold)
        {
            ActivateRage();
        }

        // 4. HURT ANIMATION (Only if shield is down)
        if (!isHurt) StartCoroutine(HurtRoutine());
    }

    void BreakShield()
    {
        isShieldBroken = true;
        currentShield = 0;
        
        // Hide the shield visual
        if(shieldVisuals != null) shieldVisuals.SetActive(false);
        
        Debug.Log("Shield BROKEN!");
        StartCoroutine(RegenerateShieldRoutine());
    }

    IEnumerator RegenerateShieldRoutine()
    {
        yield return new WaitForSeconds(BossData.shieldRegenDelay);

        if (!isDead)
        {
            isShieldBroken = false;
            currentShield = BossData.maxShield;
            
            // Show the shield visual again
            if(shieldVisuals != null) shieldVisuals.SetActive(true);
            
            Debug.Log("Shield REGENERATED!");
            
            if (isRaged) spriteRenderer.color = Color.red;
            else spriteRenderer.color = Color.white;
        }
    }

    void ActivateRage()
    {
        isRaged = true;
        Debug.Log("BOSS ENRAGED!");
        spriteRenderer.color = Color.red;
    }

    IEnumerator HurtRoutine()
    {
        isHurt = true;
        rb.linearVelocity = Vector2.zero;
        if (animator != null) animator.SetTrigger("Hurt");
        yield return new WaitForSeconds(BossData.hurtDuration);
        isHurt = false;
    }

    IEnumerator FlashShieldColor()
    {
        if (shieldVisuals == null) yield break;
        
        SpriteRenderer shieldRend = shieldVisuals.GetComponent<SpriteRenderer>();
        if(shieldRend != null)
        {
            Color original = shieldRend.color;
            shieldRend.color = new Color(original.r, original.g, original.b, 0.2f); // Dim it
            yield return new WaitForSeconds(0.1f);
            shieldRend.color = original; // Restore it
        }
    }

    protected override void Die()
    {
        if (isDead) return;
        isDead = true;
        if (animator != null) animator.SetTrigger("Die");
        if (enemyCollider != null) enemyCollider.enabled = false;
        if(shieldVisuals != null) shieldVisuals.SetActive(false);

        rb.linearVelocity = Vector2.zero;
        this.enabled = false;
        Destroy(gameObject, 2f);
    }
}