using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Required to make the Image clickable

public class WeaponSwitcher : MonoBehaviour, IPointerDownHandler
{
    [Header("Setup")]
    public WeaponShooting playerShooting; // Drag your Player object here

    [Header("Inventory")]
    public List<WeaponData> weapons;      // Drag your PistolData, SniperData, etc. here

    private Image displayImage;           // The image component on this object
    private int currentIndex = 0;

    void Start()
    {
        // Automatically find the Image component on this object
        displayImage = GetComponent<Image>();

        // 1. Sync with what the player is currently holding
        if (playerShooting.currentWeapon != null && weapons.Contains(playerShooting.currentWeapon))
        {
            currentIndex = weapons.IndexOf(playerShooting.currentWeapon);
        }

        // 2. Update the icon to show the NEXT weapon
        UpdateIcon();
    }

    // This function detects the touch/click automatically
    public void OnPointerDown(PointerEventData eventData)
    {
        SwitchWeapon();
    }

    void SwitchWeapon()
    {
        if (weapons.Count == 0 || playerShooting == null) return;

        // 1. Calculate next index
        currentIndex++;
        if (currentIndex >= weapons.Count)
        {
            currentIndex = 0; // Loop back to start
        }

        // 2. Equip the new weapon
        playerShooting.EquipWeapon(weapons[currentIndex]);

        // 3. Update the icon
        UpdateIcon();
    }

    void UpdateIcon()
    {
        if (displayImage == null || weapons.Count == 0) return;

        // Calculate what comes AFTER the current one to show as preview
        int nextIndex = currentIndex + 1;
        if (nextIndex >= weapons.Count) nextIndex = 0;

        // Set the sprite
        if (weapons[nextIndex].icon != null)
        {
            displayImage.sprite = weapons[nextIndex].icon;
            displayImage.preserveAspect = true; // Keeps the aspect ratio correct
        }
    }
}