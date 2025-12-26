using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    [Header("Weapon Configuration")]
    public WeaponData weaponData; // Drag your Pistol Data here!

    [Header("Setup")]
    public AudioSource audioSource;
    public Transform firePoint;

    private float nextShotTime;

    void Update()
    {
        if (weaponData == null) return;

        if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)) && Time.time >= nextShotTime)
        {
            Shoot();
            // Use data from ScriptableObject
            nextShotTime = Time.time + weaponData.timeBetweenShots;
        }
    }

    void Shoot()
    {
        if (firePoint == null || weaponData.bulletPrefab == null) return;

        Instantiate(weaponData.bulletPrefab, firePoint.position, firePoint.rotation);

        if (audioSource != null && weaponData.shootSound != null)
        {
            audioSource.PlayOneShot(weaponData.shootSound);
        }
    }
}