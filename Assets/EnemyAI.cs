using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [Header("Stats")]
    public float speed = 3f;
    public float health = 30f;

    [Header("References")]
    public Animator animator; // Drag the Animator component here
    public Collider2D myCollider; // Drag the BoxCollider2D here

    private Transform player;
    private bool isDead = false; // Flag to stop moving when dead

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Stop logic if we are dead
        if (isDead) return;

        // Chase the Player
        if (player != null)
        {
            Vector2 direction = (player.position - transform.position).normalized;
            transform.Translate(direction * speed * Time.deltaTime);


        }
    }

    public void TakeDamage(float damageAmount)
    {
        if (isDead) return; // Don't hurt a dead body

        health -= damageAmount;

        // Trigger Hurt Animation
        animator.SetTrigger("Hurt");

        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;

        // 1. Play Animation
        animator.SetTrigger("Die");

        // 2. Disable Physics (So you can walk over the corpse)
        myCollider.enabled = false;

        // 3. Destroy object after 1 second (Wait for animation)
        Destroy(gameObject, 1f);
    }
}