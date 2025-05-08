using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    [Header("Audio References")]
    [SerializeField] private AudioClip buttonClickSound;

    [Header("Fade Effect Settings")]
    [SerializeField] private Image fadeImage; // Reference to the black fade image
    [SerializeField] private float fadeDuration = 1f; 
    [SerializeField] private float delayBeforeLoad = 1f; 

    private AudioManager audioManager;

    private void Start()
    {
        // Find the audio manager
        audioManager = FindObjectOfType<AudioManager>();

        // Initialize cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0f; // Fully transparent
            fadeImage.color = color;
        }
    }

    public void Play()
    {
        // Play click sound
        if (audioManager != null && buttonClickSound != null)
        {
            audioManager.PlaySFX(buttonClickSound);
        }

        // Start the fade effect
        StartCoroutine(FadeAndLoadScene());
    }

    public void Quit()
    {
        // Play click sound
        if (audioManager != null && buttonClickSound != null)
        {
            audioManager.PlaySFX(buttonClickSound);
        }

        Debug.Log("Player has quit the game :)");
        Application.Quit();
    }

    private IEnumerator FadeAndLoadScene()
    {
        // Fade to black
        float elapsedTime = 0f;
        Color color = fadeImage.color;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            color.a = Mathf.Lerp(0f, 1f, elapsedTime / fadeDuration); // Fade from transparent to opaque
            fadeImage.color = color;
            yield return null;
        }

        // Ensure the fade image is fully opaque
        color.a = 1f;
        fadeImage.color = color;

        yield return new WaitForSeconds(delayBeforeLoad);

        // Load the next scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}