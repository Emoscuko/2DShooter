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
        // Logic to damage enemy goes here later
        // Destroy(gameObject); 
    }
}