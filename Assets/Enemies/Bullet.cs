using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Settings")]
    public float speed = 20f;
    public float lifeTime = 2f;
    public Rigidbody2D rb;

    void Start()
    {
        rb.linearVelocity = transform.right * speed;
        Destroy(gameObject, lifeTime);
    }

    void OnTriggerEnter2D(Collider2D hitInfo)
    {
        // FIX: Look for 'Enemy' (the Parent), so it works on Bosses AND Slimes
        Enemy enemy = hitInfo.GetComponent<Enemy>();
        
        if (enemy != null)
        {
            enemy.TakeDamage(10, transform.position); // Hits Boss or Slime
            Destroy(gameObject);
        }
        
        // Optional: Destroy on walls
        if (hitInfo.gameObject.CompareTag("Wall")) Destroy(gameObject);
    }
}