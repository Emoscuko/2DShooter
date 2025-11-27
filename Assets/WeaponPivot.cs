using UnityEngine;

public class WeaponPivot : MonoBehaviour
{
    [Header("References")]
    public PlayerMovement player;
    public SpriteRenderer gunRenderer;

    [Header("Settings")]
    public float shoulderOffset = 0.2f; // How far right the shoulder is

    void Update()
    {
        if (player.facingDir == Vector2.zero) return;

        // 1. ROTATION (Aiming)
        float angle = Mathf.Atan2(player.facingDir.y, player.facingDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        // 2. SHOULDER SWAP (The Fix!)
        // If looking Left, move pivot to Left Shoulder (-X).
        // If looking Right, move pivot to Right Shoulder (+X).
        Vector3 currentPos = transform.localPosition;
        
        if (player.facingDir.x < 0)
            currentPos.x = -shoulderOffset; // Move to Left
        else if (player.facingDir.x > 0)
            currentPos.x = shoulderOffset;  // Move to Right
            
        transform.localPosition = currentPos;


        // 3. FLIP GUN Y (Keep gun upright)
        if (player.facingDir.x < 0)
            gunRenderer.flipY = true;
        else if (player.facingDir.x > 0)
            gunRenderer.flipY = false;

        // 4. SORTING (Behind/Front)
        if (player.facingDir.y > 0.1f)
            gunRenderer.sortingOrder = -1;
        else
            gunRenderer.sortingOrder = 1;
    }
}