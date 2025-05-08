using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverController : MonoBehaviour
{
    [Header("UI Settings")]
    [SerializeField] private GameObject gameOverCanvas;
    [SerializeField] private string levelToReload = "MainScene"; // Set your scene name in Inspector

    [Header("Health Settings")]
    [SerializeField] private HealthStatus playerHealth;
    [SerializeField] private int gameOverThreshold = 6;

    private bool isGameOver = false;

    public static bool IsGameOver { get; private set; }

    private void Start()
    {
        // Initialize references if not set
        if (playerHealth == null)
        {
            playerHealth = FindObjectOfType<HealthStatus>();
            if (playerHealth == null) Debug.LogError("HealthStatus component not found in scene!");
        }

        // Ensure canvas is hidden at start
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }
        else
        {
            Debug.LogError("Game Over Canvas not assigned!");
        }

        IsGameOver = false;
    }

    private void Update()
    {
        if (isGameOver || playerHealth == null) return;

        if (playerHealth.GetHitCount() > gameOverThreshold)
        {
            TriggerGameOver();
        }
    }

    private void TriggerGameOver()
    {
        isGameOver = true;
        IsGameOver = true;

        // Pause game physics
        Time.timeScale = 0f;

        // Show cursor
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Show game over UI
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(true);
        }
    }

    public void RestartLevel()
    {
        // Reset game state before loading
        isGameOver = false;
        IsGameOver = false;
        Time.timeScale = 1f;

        // Hide canvas immediately
        if (gameOverCanvas != null)
        {
            gameOverCanvas.SetActive(false);
        }

        // Reload the current scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitToMenu()
    {
        Time.timeScale = 1f;
        IsGameOver = false;
        SceneManager.LoadScene("MainMenu");
    }

    // Called when the scene is loaded
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset game state
        isGameOver = false;
        IsGameOver = false;
        Time.timeScale = 1f;

        // Ensure cursor is locked for gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Register scene load callback
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // Unregister callback
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}