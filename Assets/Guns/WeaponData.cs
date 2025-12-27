using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon", menuName = "Scriptable Objects/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Visuals")]
    public string weaponName;
    public GameObject weaponPrefab;
    public Sprite icon;
    public GameObject bulletPrefab;
    
    // NEW: How far from the player center should this gun sit?
    public float weaponHoldDistance = 1.2f; 

    [Header("Stats")]
    public float fireRate;      
    public int damage;          
    public float bulletSpeed;   
    public float bulletLifetime; 

    [Header("Audio")]
    public AudioClip shootSound;
}