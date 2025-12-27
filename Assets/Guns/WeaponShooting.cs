using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    [Header("Weapon Setup")]
    public WeaponData currentWeapon; // Drag your PistolData or SniperData here!
    public Transform firePoint;      // Keep this assigned
    public SpriteRenderer weaponRenderer; // Assign the gun sprite to change it visually

    [Header("Audio")]
    public AudioSource audioSource;

    private float nextFireTime;

    void Update()
    {
        // PC Input (Mobile input should call Shoot() directly via button)
        // We check currentWeapon.fireRate now
        if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)) && Time.time >= nextFireTime)
        {
            AttemptShoot();
        }
    }

    public void AttemptShoot()
    {
        if (currentWeapon == null) return;

        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + currentWeapon.fireRate;
            Shoot();
        }
    }

    void Shoot()
    {
        if (currentWeapon.bulletPrefab == null || firePoint == null) return;

        // 1. Create the bullet
        GameObject bulletObj = Instantiate(currentWeapon.bulletPrefab, firePoint.position, firePoint.rotation);

        // 2. Pass the DATA to the bullet
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(currentWeapon.damage, currentWeapon.bulletSpeed, currentWeapon.bulletLifetime);
        }

        // 3. Play Sound from the Data
        if (audioSource != null && currentWeapon.shootSound != null)
        {
            // Randomize pitch slightly for better feel
            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(currentWeapon.shootSound);
        }
    }

    // Call this to change weapons (e.g. from a Pickup or Button)
    public void EquipWeapon(WeaponData newWeapon)
    {
        currentWeapon = newWeapon;
        if (weaponRenderer != null)
        {
            weaponRenderer.sprite = newWeapon.weaponSprite;
        }
    }
}