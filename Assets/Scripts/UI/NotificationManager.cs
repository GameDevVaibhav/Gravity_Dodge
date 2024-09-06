using System.Collections;
using UnityEngine;
using TMPro;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance; // Singleton instance
    public TextMeshProUGUI notificationText; // Reference to the UI Text element
    public CanvasGroup canvasGroup; // CanvasGroup for handling fade in/out
    public float displayDuration = 2f; // How long the notification stays on screen
    public float fadeDuration = 0.5f; // How long it takes to fade in/out

    private void Awake()
    {
        // Ensure there's only one instance of the NotificationManager
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Method to show a notification
    public void ShowNotification(string message)
    {
        StopAllCoroutines(); // Stop any existing fade animations
        notificationText.text = message; // Set the notification text
        StartCoroutine(DisplayNotification());
    }

    // Coroutine to handle displaying and fading the notification
    private IEnumerator DisplayNotification()
    {
        // Fade in
        yield return StartCoroutine(FadeIn());

        // Wait for the display duration
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        yield return StartCoroutine(FadeOut());
    }

    private IEnumerator FadeIn()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 1f;
    }

    private IEnumerator FadeOut()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            canvasGroup.alpha = Mathf.Clamp01(1f - (elapsedTime / fadeDuration));
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        canvasGroup.alpha = 0f;
    }
}
