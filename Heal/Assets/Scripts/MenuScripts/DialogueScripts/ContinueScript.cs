using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ContinueScript : MonoBehaviour
{
    [Header("Fade Effect Settings")]
    [SerializeField] private Image fadeImage; // Reference to the black fade image
    [SerializeField] private float fadeDuration = 1f; // Duration of the fade effect
    [SerializeField] private float delayBeforeLoad = 1f; // Delay after fade before loading the next scene

    private void Start()
    {
        // Ensure the fade image is transparent at the start
        if (fadeImage != null)
        {
            Color color = fadeImage.color;
            color.a = 0f; // Fully transparent
            fadeImage.color = color;
        }
    }

    // Make sure this function is public
    public void OnContinueClicked()
    {
        StartCoroutine(FadeAndLoadScene());
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

        // Wait for the delay before loading the next scene
        yield return new WaitForSeconds(delayBeforeLoad);

        // Load the next scene in the build index
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}