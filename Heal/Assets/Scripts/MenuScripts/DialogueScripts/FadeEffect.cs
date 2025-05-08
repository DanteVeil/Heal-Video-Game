using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FadeEffect : MonoBehaviour
{
    public CanvasGroup fadeCanvas;
    public float fadeDuration = 1f;

    void Start()
    {
        // Auto-fade-out when scene loads
        if (fadeCanvas != null && fadeCanvas.alpha == 1f)
        {
            FadeOut();
        }
    }

    public void FadeIn() => StartCoroutine(Fade(1f)); // To black
    public void FadeOut() => StartCoroutine(Fade(0f)); // To clear

    private IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadeCanvas.alpha;
        float time = 0f;

        while (time < fadeDuration)
        {
            time += Time.deltaTime;
            fadeCanvas.alpha = Mathf.Lerp(startAlpha, targetAlpha, time / fadeDuration);
            yield return null;
        }

        fadeCanvas.alpha = targetAlpha;
    }
}