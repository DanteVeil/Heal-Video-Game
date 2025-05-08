using UnityEngine;
using UnityEngine.SceneManagement;

public class HorrorBackgroundMusic : MonoBehaviour
{
    [Header("Music Settings")]
    public AudioClip levelMusic;
    [Range(0, 1)] public float volume = 0.7f;
    public bool loopMusic = true;

    private AudioSource audioSource;
    private static HorrorBackgroundMusic instance;
    private string currentSceneName;

    void Awake()
    {
        // Singleton pattern to prevent duplicates
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            currentSceneName = SceneManager.GetActiveScene().name;

            SetupAudioSource();
            SceneManager.activeSceneChanged += OnSceneChanged;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void SetupAudioSource()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.clip = levelMusic;
        audioSource.volume = volume;
        audioSource.loop = loopMusic;
        audioSource.spatialBlend = 0; // Force 2D sound
        audioSource.Play();
    }

    void OnSceneChanged(Scene current, Scene next)
    {
        // Destroy music player if returning to main menu
        if (next.name == "MainMenu" || next.name != currentSceneName)
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        // Clean up event subscription
        if (instance == this)
        {
            SceneManager.activeSceneChanged -= OnSceneChanged;
        }
    }
}