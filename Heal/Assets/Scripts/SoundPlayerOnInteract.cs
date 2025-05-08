using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundPlayerOnInteract : MonoBehaviour
{
    [Header("Audio Settings")]
    [SerializeField] private AudioClip soundToPlay;
    [SerializeField] private float volume = 1f;
    [SerializeField] private bool loop = true;

    private AudioSource audioSource;
    private bool hasBeenActivated = false;

    void Awake()
    {
        // Get or create audio source
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Configure audio source
        audioSource.clip = soundToPlay;
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.playOnAwake = false;

        // Add scene unloaded listener
        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    private void OnDestroy()
    {
        // Clean up scene unloaded listener
        SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    // Public method to be called by InteractToActivate
    public void ActivateSound()
    {
        if (!hasBeenActivated && audioSource != null && soundToPlay != null)
        {
            audioSource.Play();
            hasBeenActivated = true;
            Debug.Log($"Sound '{soundToPlay.name}' activated", this);
        }
    }

    // Stop sound when leaving the scene
    private void OnSceneUnloaded(Scene scene)
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
            Debug.Log($"Sound '{soundToPlay.name}' stopped due to scene unload", this);
        }
    }
}