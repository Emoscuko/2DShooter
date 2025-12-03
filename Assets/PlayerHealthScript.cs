using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    public int maxHealth = 6; // We use 'int' (whole numbers) for ticks
    private int currentHealth;

    [Header("UI References")]
    public Image healthDisplay;      // The UI Image object on the Canvas
    public Sprite[] healthSprites;   // The collection of 6 pictures

    void Start()
    {
        currentHealth = maxHealth;
        UpdateHealthUI();
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;

        // Prevent health from going below 0 (negative numbers crash the array)
        if (currentHealth < 0) currentHealth = 0;

        UpdateHealthUI();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void UpdateHealthUI()
    {
        // LOGIC: Map the health number directly to the image number.
        // If currentHealth is 5, it picks healthSprites[5]
        // If currentHealth is 0, it picks healthSprites[0]
        if (healthDisplay != null && healthSprites.Length > currentHealth)
        {
            healthDisplay.sprite = healthSprites[currentHealth];
        }
    }

    void Die()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}