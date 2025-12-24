using UnityEngine;
using System.Collections; // Required for Coroutines

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 5f;

    [Header("Dash Settings")]
    public float dashSpeed = 15f;    // Speed during the dash
    public float dashDuration = 0.2f; // How long the dash lasts
    public float dashCooldown = 1f;   // Time between dashes
    private bool isDashing;           // Flag to stop normal movement
    private bool canDash = true;      // Cooldown flag

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    [Header("Controls")]
    public VirtualJoystick moveJoystick;

    [HideInInspector] public Vector2 facingDir = Vector2.down;
    private Vector2 movement;

    void Update()
    {
        // 1. Skip normal input if we are currently dashing
        if (isDashing) return;

        // 2. Get Input (Keyboard + Joystick)
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (moveJoystick != null && moveJoystick.inputVector != Vector2.zero)
        {
            movement = moveJoystick.inputVector;
        }

        // 3. Dash Input Check (Space Bar)
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartCoroutine(Dash());
        }

        // 4. Flip Logic
        if (movement.x > 0)
            spriteRenderer.flipX = false;
        else if (movement.x < 0)
            spriteRenderer.flipX = true;

        // 5. Update Animator
        if (animator != null)
        {
            animator.SetFloat("Speed", movement.sqrMagnitude);

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
        // 6. Physics Movement (Skip if dashing)
        if (isDashing) return;

        Vector2 finalMove = movement;
        if (finalMove.magnitude > 1) finalMove.Normalize();

        rb.linearVelocity = finalMove * moveSpeed;
    }

    // Coroutine to handle the dash physics and cooldown
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        // Determine dash direction: Use current movement if moving, otherwise use facing direction
        Vector2 dashDir = movement.sqrMagnitude > 0.001f ? movement.normalized : facingDir;

        // Apply high velocity for the duration
        rb.linearVelocity = dashDir * dashSpeed;

        // Optional: If you have a dash animation, trigger it here
        // animator.SetTrigger("Dash");

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        // Wait for cooldown
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}