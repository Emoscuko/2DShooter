using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    [Header("Weapon Setup")]
    public WeaponData currentWeapon; // Drag your Pistol Data here by default
    public Transform firePoint;      // Where the bullet comes out
    public SpriteRenderer weaponRenderer; // Reference to the Gun Sprite inside the player
    [Header("Audio")]
    public AudioSource audioSource;
    private float nextFireTime = 0f;

    void Start()
    {
        // Ensure the visual matches the data on game start
        if (currentWeapon != null)
            EquipWeapon(currentWeapon);
    }

    void Update()
    {
        // PC Input
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
    }

    // Call this from your Mobile Button or PC Input
    public void Shoot()
    {
        if (currentWeapon == null) return;

        if (Time.time >= nextFireTime)
        {
            // Calculate next time allowed to shoot
            nextFireTime = Time.time + currentWeapon.fireRate;

            FireBullet();
            PlayShootSound();
        }
    }
    void PlayShootSound()
    {
        if (currentWeapon.shootSound != null && audioSource != null)
        {
            // PlayOneShot is best for shooting because sounds can overlap 
            // without cutting each other off
            audioSource.PlayOneShot(currentWeapon.shootSound);
        }
    }
    void FireBullet()
    {
        // 1. Create the bullet
        GameObject bulletObj = Instantiate(currentWeapon.bulletPrefab, firePoint.position, firePoint.rotation);

        // 2. Get the bullet script
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();

        // 3. Inject the stats from the Weapon Data
        if (bulletScript != null)
        {
            bulletScript.Initialize(
                currentWeapon.damage,
                currentWeapon.bulletSpeed,
                firePoint.right, // Assuming gun points right
                currentWeapon.bulletLifetime
            );
        }
    }

    // Call this to switch weapons
    public void EquipWeapon(WeaponData newWeapon)
    {
        currentWeapon = newWeapon;

        // Update the sprite to look like the new gun
        if (weaponRenderer != null)
        {
            weaponRenderer.sprite = newWeapon.weaponSprite;
        }
    }
}