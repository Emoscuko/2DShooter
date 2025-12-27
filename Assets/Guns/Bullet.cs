using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage;
    private Rigidbody2D rb;

    // This function is called by the Gun when the bullet is created
    public void Initialize(int weaponDamage, float speed, Vector2 direction, float lifetime)
    {
        rb = GetComponent<Rigidbody2D>();
        damage = weaponDamage;

        // Destroy bullet after X seconds so it doesn't fly forever
        Destroy(gameObject, lifetime);

        // Set velocity
        rb.linearVelocity = direction * speed;

        // Rotate bullet to face direction
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Example Enemy Hit Logic
        if (collision.CompareTag("Enemy"))
        {
            // Assuming you have an Enemy script with TakeDamage
            // collision.GetComponent<Enemy>()?.TakeDamage(damage); 
            Destroy(gameObject);
        }
        else if (!collision.CompareTag("Player") && !collision.CompareTag("Bullet"))
        {
            // Destroy on walls
            Destroy(gameObject);
        }
    }
}