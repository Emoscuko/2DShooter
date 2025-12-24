using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource audioSource;  // The component we just added
    public AudioClip shootSound;     // The actual MP3/WAV file
    [Header("Prefabs")]
    public GameObject bulletPrefab; // Drag your 'Bullet' blue prefab here

    [Header("Settings")]
    public Transform firePoint;     // Drag the 'FirePoint' empty object here
    public float timeBetweenShots = 0.2f; // Lower number = Faster shooting

    private float nextShotTime;

    void Update()
    {
        // Shoot if Mouse Held Down OR Spacebar Held Down
        // AND if enough time has passed since the last shot
        if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)) && Time.time >= nextShotTime)
        {
            Shoot();
            nextShotTime = Time.time + timeBetweenShots;
        }
    }

    void Shoot()
    {
        if (firePoint == null || bulletPrefab == null) return;

        // Spawn the bullet at the FirePoint position, matching the FirePoint's rotation
        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}