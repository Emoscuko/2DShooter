using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // Required for the Fade logic

public class MainMenu : MonoBehaviour
{
    [Header("UI Settings")]
    public CanvasGroup mainCanvasGroup; // Drag your Canvas here
    public float fadeDuration = 1.5f;   // How long the fade takes

    void Start()
    {
        // 1. Ensure it starts invisible
        if (mainCanvasGroup != null)
        {
            mainCanvasGroup.alpha = 0;
            // 2. Start the fade-in animation
            StartCoroutine(FadeIn());
        }
    }

    IEnumerator FadeIn()
    {
        float timer = 0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            // Math trick to go smoothly from 0 to 1
            mainCanvasGroup.alpha = timer / fadeDuration; 
            yield return null; // Wait for the next frame
        }

        mainCanvasGroup.alpha = 1f; // Ensure it finishes at 100% visible
    }

    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}