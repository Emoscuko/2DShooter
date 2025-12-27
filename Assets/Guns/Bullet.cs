using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage; // Not public, set by Initialize
    private Rigidbody2D rb;

    // Call this from WeaponShooting to set the stats
    public void Initialize(int weaponDamage, float speed, float lifetime)
    {
        rb = GetComponent<Rigidbody2D>();
        damage = weaponDamage;

        // Set velocity using the speed passed in
        // Note: transform.right works because we rotate the bullet when spawning it
        rb.linearVelocity = transform.right * speed;

        // Destroy bullet automatically after lifetime expires
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        Enemy enemy = hitInfo.GetComponent<Enemy>();

        if (enemy != null)
        {
            // Use the variable 'damage' instead of hardcoded 10
            enemy.TakeDamage(damage, transform.position);
            Destroy(gameObject);
        }

        // Destroy on walls (make sure your walls have the tag "Wall" or "Ground")
        if (hitInfo.gameObject.CompareTag("Wall") || hitInfo.gameObject.CompareTag("Ground"))
        {
            Destroy(gameObject);
        }
    }
}