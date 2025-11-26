using UnityEngine;

public class WeaponPivot : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement player;       // Drag the Player object here
    public SpriteRenderer gunRenderer;  // Drag the Pistol Sprite object here

    void Update()
    {
        // 1. ROTATION
        // Only update rotation if we have a direction (prevents snapping to 0 when idle)
        if (player.facingDir != Vector2.zero)
        {
            // Math to turn Direction (X,Y) into an Angle (Degrees)
            float angle = Mathf.Atan2(player.facingDir.y, player.facingDir.x) * Mathf.Rad2Deg;
            
            // Apply rotation to the WeaponHolder
            transform.rotation = Quaternion.Euler(0, 0, angle);
        }

        // 2. FLIP LOGIC
        // If we are looking left (negative X), the gun sprite rotates upside down.
        // We flip the Y axis of the sprite to fix this visually.
        if (player.facingDir.x < 0)
            gunRenderer.flipY = true;
        else if (player.facingDir.x > 0)
            gunRenderer.flipY = false;

        // 3. SORTING LAYER
        // Put gun behind player when walking UP, in front when walking DOWN
        if (player.facingDir.y > 0.1f)
            gunRenderer.sortingOrder = -1; // Behind Player
        else
            gunRenderer.sortingOrder = 1;  // In Front of Player
    }
}