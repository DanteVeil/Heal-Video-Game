// MessageDisplay.cs
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MessageDisplay : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Text messageText;  // Reference to UI Text component

    [Header("Display Settings")]
    [SerializeField] private float displayDuration = 3f;
    [SerializeField] private float fadeSpeed = 2f;

    private CanvasGroup canvasGroup;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        // Try to get the Text component if not assigned
        if (messageText == null)
        {
            messageText = GetComponentInChildren<Text>();
            if (messageText == null)
            {
                Debug.LogError("MessageDisplay: No Text component found! Please assign a UI Text component.");
            }
        }

        // Get or add CanvasGroup
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
            Debug.Log("MessageDisplay: Added CanvasGroup component.");
        }

        // Initially hide the message
        canvasGroup.alpha = 0;

        // Ensure the object is set up correctly
        ValidateSetup();
    }

    private void ValidateSetup()
    {
        // Check if we're part of a Canvas
        Canvas parentCanvas = GetComponentInParent<Canvas>();
        if (parentCanvas == null)
        {
            Debug.LogError("MessageDisplay: This component must be child of a Canvas!");
        }

        if (messageText != null)
        {
            Debug.Log("MessageDisplay: System initialized successfully.");
        }
    }

    public void ShowMessage(string message)
    {
        if (messageText == null)
        {
            Debug.LogError("MessageDisplay: Cannot show message - Text component is missing!");
            return;
        }

        Debug.Log($"MessageDisplay: Showing message: {message}");

        // Set the message text
        messageText.text = message;

        // Stop any existing fade coroutine
        if (fadeCoroutine != null)
        {
            StopCoroutine(fadeCoroutine);
        }

        // Start new fade coroutine
        fadeCoroutine = StartCoroutine(FadeMessage());
    }

    private IEnumerator FadeMessage()
    {
        // Fade in
        canvasGroup.alpha = 1;

        // Wait for display duration
        yield return new WaitForSeconds(displayDuration);

        // Fade out
        while (canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
    }
}