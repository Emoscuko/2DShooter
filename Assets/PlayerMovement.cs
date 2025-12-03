using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 5f;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer; 

    [Header("Controls")]
    public VirtualJoystick moveJoystick; // Drag your Joystick_Move here in Inspector

    // Hidden variables for logic
    [HideInInspector] public Vector2 facingDir = Vector2.down;
    private Vector2 movement;

    void Update()
    {
        // 1. Get Input (Hybrid: Keyboard + Joystick)
        // First, check keyboard
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // If keyboard is not being used, check Joystick
        // (This allows you to debug on PC with WASD, but use Joystick on Mobile)
        if (moveJoystick != null && moveJoystick.inputVector != Vector2.zero)
        {
            movement = moveJoystick.inputVector;
        }

        // 2. Flip Logic (The Mirror Trick)
        // We check 'movement.x' regardless of whether it came from Joystick or Keyboard
        if (movement.x > 0)
            spriteRenderer.flipX = false; // Face Right
        else if (movement.x < 0)
            spriteRenderer.flipX = true;  // Face Left

        // 3. Update Animator
        if (animator != null)
        {
            // Use 'sqrMagnitude' for performance (cheaper than calculating exact length)
            animator.SetFloat("Speed", movement.sqrMagnitude);

            // Only update "Facing Direction" memory if we are actually moving
            if (movement.sqrMagnitude > 0.01f)
            {
                facingDir = movement.normalized;
                animator.SetFloat("InputX", facingDir.x);
                animator.SetFloat("InputY", facingDir.y);
            }
        }
    }

    void FixedUpdate()
    {
        // 4. Physics Movement
        Vector2 finalMove = movement;
        if(finalMove.magnitude > 1) finalMove.Normalize();

        // OLD WAY (Can force through walls):
        // rb.MovePosition(rb.position + finalMove * moveSpeed * Time.fixedDeltaTime);

        // NEW WAY (Respects walls perfectly):
        rb.linearVelocity = finalMove * moveSpeed;
    }
}