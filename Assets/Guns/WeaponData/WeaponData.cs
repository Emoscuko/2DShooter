using UnityEngine;

[CreateAssetMenu(fileName = "NewWeaponData", menuName = "ScriptableObjects/Weapon Data")]
public class WeaponData : ScriptableObject
{
    [Header("Settings")]
    public GameObject bulletPrefab;
    public float timeBetweenShots = 0.2f;
    
    [Header("Audio")]
    public AudioClip shootSound;
}