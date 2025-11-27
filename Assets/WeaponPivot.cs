using UnityEngine;

public class WeaponPivot : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement player;
    public SpriteRenderer gunRenderer;
    public VirtualJoystick aimJoystick;

    void Update()
    {
        Vector2 aimDirection = Vector2.zero;

        // 1. GET INPUT (Mouse or Joystick)
        if (aimJoystick != null && aimJoystick.inputVector != Vector2.zero)
        {
            aimDirection = aimJoystick.inputVector;
        }
        else
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            aimDirection = (mousePos - player.transform.position).normalized;
        }

        // 2. ORBIT ROTATION (The Invisible Circle)
        // We simply rotate the parent. The child (Gun) orbits because it is offset.
        float angle = Mathf.Atan2(aimDirection.y, aimDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // 3. FLIP GUN (Keep it upright)
        // If aiming left (angle is roughly between 90 and -90 degrees on the left side)
        // We flip the sprite so it doesn't look upside down.
        if (aimDirection.x < 0)
            gunRenderer.flipY = true;
        else
            gunRenderer.flipY = false;

        // 4. VISUAL SORTING (Optional Polish)
        // Put gun behind player when aiming UP
        if (aimDirection.y > 0.1f)
            gunRenderer.sortingOrder = -1; // Behind
        else
            gunRenderer.sortingOrder = 1;  // Front
    }
}