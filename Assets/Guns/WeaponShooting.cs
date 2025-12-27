using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    [Header("Setup")]
    public WeaponData currentWeapon; 
    public Transform weaponPivot;
    
    [Header("Audio")]
    public AudioSource audioSource;

    private GameObject currentGunInstance;
    private Transform currentFirePoint;
    private float nextFireTime;

    void Awake()
    {
        if (audioSource == null) audioSource = GetComponent<AudioSource>();
        if (currentWeapon != null) EquipWeapon(currentWeapon);
    }

    void Update()
    {
        // PC Input
        if ((Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space)))
        {
            AttemptShoot();
        }
    }

    public void AttemptShoot()
    {
        if (currentWeapon == null || Time.time < nextFireTime) return;

        nextFireTime = Time.time + currentWeapon.fireRate;
        Shoot();
    }

    void Shoot()
    {
        if (currentWeapon.bulletPrefab == null || currentFirePoint == null) return;

        GameObject bulletObj = Instantiate(currentWeapon.bulletPrefab, currentFirePoint.position, currentFirePoint.rotation);
        
        Bullet bulletScript = bulletObj.GetComponent<Bullet>();
        if (bulletScript != null)
        {
            bulletScript.Initialize(currentWeapon.damage, currentWeapon.bulletSpeed, currentWeapon.bulletLifetime);
        }

        if (audioSource != null && currentWeapon.shootSound != null)
        {
            audioSource.PlayOneShot(currentWeapon.shootSound);
        }
    }

    public void EquipWeapon(WeaponData newWeapon)
    {
        if (newWeapon == null || newWeapon.weaponPrefab == null) return;

        currentWeapon = newWeapon;

        // 1. Destroy old gun
        if (currentGunInstance != null) Destroy(currentGunInstance);

        // 2. Instantiate new gun
        currentGunInstance = Instantiate(newWeapon.weaponPrefab, weaponPivot);
        
        // 3. APPLY THE OFFSET HERE (This creates the circle movement)
        // We move it to the right along the local X axis
        currentGunInstance.transform.localPosition = new Vector3(newWeapon.weaponHoldDistance, 0, 0);
        currentGunInstance.transform.localRotation = Quaternion.identity;

        // 4. Find FirePoint
        currentFirePoint = currentGunInstance.transform.Find("FirePoint");

        if (currentFirePoint == null)
        {
            Debug.LogError($"Weapon '{newWeapon.weaponName}' prefab is missing a child named 'FirePoint'!");
        }
    }
}