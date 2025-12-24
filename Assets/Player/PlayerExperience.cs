using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    [Header("Level Stats")]
    public int currentLevel = 1;
    public float currentXP = 0;
    public float xpToNextLevel = 100;

    public void GainExperience(float amount)
    {
        currentXP += amount;
        Debug.Log($"Gained {amount} XP! Total: {currentXP}");

        // Level Up Check
        while (currentXP >= xpToNextLevel)
        {
            LevelUp();
        }
    }

    void LevelUp()
    {
        currentXP -= xpToNextLevel;
        currentLevel++;

        // Increase XP requirement by 20% each level (Exponential curve)
        xpToNextLevel = Mathf.RoundToInt(xpToNextLevel * 1.2f);

        Debug.Log($"LEVEL UP! You are now level {currentLevel}");

        // Optional: Add logic here to increase PlayerHealth max HP, etc.
    }
}