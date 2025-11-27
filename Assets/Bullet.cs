using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 20f;
    public float lifeTime = 2f; // Auto-destroy after 2 seconds
    public Rigidbody2D rb;

    void Start()
    {
        // Fly forward (Red Axis) instantly when spawned
        rb.linearVelocity = transform.right * speed;

        // Destroy bullet after X seconds so game doesn't lag
        Destroy(gameObject, lifeTime);
    }

    
    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // Check if we hit something with the "Enemy" component
        EnemyAI enemy = hitInfo.GetComponent<EnemyAI>();
        
        if (enemy != null)
        {
            enemy.TakeDamage(10); // Deal 10 damage
            Destroy(gameObject);  // Destroy the bullet
        }
        
        // Optional: Destroy bullet on walls too
        // if (hitInfo.name != "Player") Destroy(gameObject);
    }
}