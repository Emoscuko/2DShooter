using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    [Header("Joystick References")]
    public VirtualJoystick moveJoystick; // Reference to Movement Joystick
    public VirtualJoystick aimJoystick;  // Reference to Aiming/Shooting Joystick

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip shootSound;

    [Header("Prefabs")]
    public GameObject bulletPrefab;

    [Header("Settings")]
    public Transform firePoint;
    public float timeBetweenShots = 0.2f;

    private float nextShotTime;

    void Update()
    {
        bool canShoot = false;

        // 1. Check if we are pressing the movement joystick
        bool isMoving = moveJoystick != null && moveJoystick.isPressed;

        // 2. Check if we are pressing the aim/fire joystick
        bool isAiming = aimJoystick != null && aimJoystick.isPressed;

        // 3. Logic: If moving, ONLY the aim joystick works.
        // If NOT moving, mouse/spacebar also work.
        if (isMoving)
        {
            if (isAiming) canShoot = true;
        }
        else
        {
            if (isAiming || Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space))
            {
                canShoot = true;
            }
        }

        // 4. Final check with timer
        if (canShoot && Time.time >= nextShotTime)
        {
            Shoot();
            nextShotTime = Time.time + timeBetweenShots;
        }
    }

    void Shoot()
    {
        if (firePoint == null || bulletPrefab == null) return;

        Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

        if (audioSource != null && shootSound != null)
        {
            audioSource.PlayOneShot(shootSound);
        }
    }
}