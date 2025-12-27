using UnityEngine;

public class WeaponPivot : MonoBehaviour
{
    public VirtualJoystick aimJoystick; // Assign your right-hand joystick here

    void Update()
    {
        Vector2 direction = Vector2.zero;

        // 1. Priority: Mobile Joystick
        if (aimJoystick != null && aimJoystick.inputVector != Vector2.zero)
        {
            direction = aimJoystick.inputVector;
        }
        // 2. Fallback: PC Mouse
        else
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            direction = (mousePos - transform.position).normalized;
        }

        // 3. Apply Rotation
        if (direction != Vector2.zero)
        {
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            // Flip the gun sprite so it's not upside down when looking left
            if (angle > 90 || angle < -90)
                transform.localScale = new Vector3(1, -1, 1);
            else
                transform.localScale = new Vector3(1, 1, 1);
        }
    }
}