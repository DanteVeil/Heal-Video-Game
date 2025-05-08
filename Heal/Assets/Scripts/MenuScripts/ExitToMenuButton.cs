using UnityEngine;
using UnityEngine.SceneManagement;

public class ExitToMenuButton : MonoBehaviour
{
    [SerializeField] private int menuSceneIndex = 0; 

    public void ReturnToMainMenu()
    {
        Time.timeScale = 1f;

        // Reset cursor state to visible and normal
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // Load the menu scene
        SceneManager.LoadScene(menuSceneIndex);
    }
}