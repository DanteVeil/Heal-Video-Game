using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject pauseCanvas;    // main pause menu canvas
    public GameObject optionsCanvas;  // Separate options canvas

    private void Start()
    {
        // Ensure both menus start hidden
        pauseCanvas.SetActive(false);
        optionsCanvas.SetActive(false);
    }

    private void Update()
    {
        // Check if game is over before allowing pause functionality
        if (GameOverController.IsGameOver)
            return;

        // Handle ESC key for pause toggle existing unchanged functionality
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (optionsCanvas.activeSelf)
            {
                // If options are open, close them and return to pause menu
                CloseOptions();
            }
            else
            {
                //  original pause toggle logic
                TogglePause();
            }
        }
    }

    //  original pause toggle
    public static bool IsGamePaused { get; private set; }

    private void TogglePause()
    {
        IsGamePaused = !pauseCanvas.activeSelf;
        pauseCanvas.SetActive(IsGamePaused);
        Time.timeScale = IsGamePaused ? 0f : 1f;
        Cursor.lockState = IsGamePaused ? CursorLockMode.None : CursorLockMode.Locked;
        Cursor.visible = IsGamePaused;
    }

    //update OpenOptions/CloseOptions to ensure cursor stays unlocked:
    public void OpenOptions()
    {
        pauseCanvas.SetActive(false);
        optionsCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void CloseOptions()
    {
        optionsCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}