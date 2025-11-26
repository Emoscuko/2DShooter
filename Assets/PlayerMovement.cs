using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 5f;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer; // Used to flip the sprite

    // We make this public so the Weapon script can read it
    [HideInInspector] public Vector2 facingDir = Vector2.down;
    private Vector2 movement;

    void Update()
    {
        // 1. Get Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // 2. Flip Logic (The Mirror Trick)
        if (movement.x > 0)
            spriteRenderer.flipX = false; // Face Right
        else if (movement.x < 0)
            spriteRenderer.flipX = true;  // Face Left (Flip)

        // 3. Update Animator
        if (animator != null)
        {
            animator.SetFloat("Speed", movement.sqrMagnitude);

            // Only update direction memory if we are actually moving
            if (movement.sqrMagnitude > 0)
            {
                facingDir = movement.normalized;
                animator.SetFloat("InputX", movement.x);
                animator.SetFloat("InputY", movement.y);
            }
        }
    }

    void FixedUpdate()
    {
        // Physics Movement
        rb.MovePosition(rb.position + movement.normalized * moveSpeed * Time.fixedDeltaTime);
    }
}