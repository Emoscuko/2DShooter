using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Settings")]
    public float moveSpeed = 5f;

    [Header("Dash Settings")]
    public float dashSpeed = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;

    [Header("Components")]
    public Rigidbody2D rb;
    public Animator animator;
    public SpriteRenderer spriteRenderer;

    [Header("Controls")]
    public VirtualJoystick moveJoystick;

    // Logic variables
    [HideInInspector] public Vector2 facingDir = Vector2.down;
    private Vector2 movement;
    private bool isDashing;
    private bool canDash = true;

    void Update()
    {
        // 0. Prevent inputs while dashing so player can't change direction mid-dash
        if (isDashing) return;

        // 1. Get Input
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (moveJoystick != null && moveJoystick.inputVector != Vector2.zero)
        {
            movement = moveJoystick.inputVector;
        }

        // 2. Dash Input for PC (Left Shift)
        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            AttemptDash();
        }

        // 3. Flip Logic
        if (movement.x > 0)
            spriteRenderer.flipX = false;
        else if (movement.x < 0)
            spriteRenderer.flipX = true;

        // 4. Update Animator
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
        if (isDashing) return; // Don't run normal movement logic while dashing

        Vector2 finalMove = movement;
        if (finalMove.magnitude > 1) finalMove.Normalize();

        rb.linearVelocity = finalMove * moveSpeed;
    }

    // Call this from your Mobile Button or Spacebar
    public void AttemptDash()
    {
        if (canDash && !isDashing)
        {
            StartCoroutine(DashCoroutine());
        }
    }

    private IEnumerator DashCoroutine()
    {
        isDashing = true;
        canDash = false;

        // Determine dash direction: use current movement, or facing direction if standing still
        Vector2 dashDirection = movement;
        if (dashDirection == Vector2.zero) dashDirection = facingDir;

        // Apply Dash Velocity
        rb.linearVelocity = dashDirection.normalized * dashSpeed;

        // Optional: Trigger Dash Animation here if you have one
        // animator.SetTrigger("Dash"); 

        yield return new WaitForSeconds(dashDuration);

        isDashing = false;

        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}