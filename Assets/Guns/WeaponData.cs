using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Scriptable Objects/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Visuals")]
    public string weaponName;
    public Sprite weaponSprite; // The picture of the gun
    public GameObject bulletPrefab; // The specific bullet this gun shoots

    [Header("Stats")]
    public float fireRate;      // Time between shots (0.2 for pistol, 1.0 for sniper)
    public int damage;          // How much hp it removes
    public float bulletSpeed;   // How fast the bullet flies
    public float bulletLifetime; // How long before bullet disappears

    [Header("Audio")]
    public AudioClip shootSound;
    
    //public AudioClip emptyClip;

    //public AudioClip reloadSound;
}