using System.Collections; // Add this line for IEnumerator
using UnityEngine;
using UnityEngine.UI; // Add this line for UI components
using UnityEngine.SceneManagement;

public class DialogueController : MonoBehaviour
{
    [Header("UI References")]
    public Text dialogueText; // Reference to the Text UI element
    public Button continueButton; // Reference to the Continue button

    [Header("Dialogue Settings")]
    [SerializeField] private string[] dialogueLines; // Array of dialogue lines
    [SerializeField] private float typewriterSpeed = 0.05f; // Speed of the typewriter effect (in seconds per character)

    private int currentLineIndex = 0; // Index of the current dialogue line
    private bool isTyping = false; // Flag to check if the typewriter effect is running

    private void Start()
    {
        // Hide the continue button initially
        continueButton.gameObject.SetActive(false);

        // Start displaying the first line of dialogue
        if (dialogueLines.Length > 0)
        {
            StartCoroutine(TypeText(dialogueLines[currentLineIndex]));
        }
    }

    private void Update()
    {
        // Allow the player to skip the typewriter effect by pressing a key (e.g., Space or Left Mouse Button)
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                // Skip the typewriter effect and show the full line immediately
                StopAllCoroutines();
                dialogueText.text = dialogueLines[currentLineIndex];
                isTyping = false;
                continueButton.gameObject.SetActive(true);
            }
            else if (currentLineIndex < dialogueLines.Length - 1)
            {
                // Move to the next line of dialogue
                currentLineIndex++;
                StartCoroutine(TypeText(dialogueLines[currentLineIndex]));
                continueButton.gameObject.SetActive(false);
            }
        }
    }

    private IEnumerator TypeText(string line)
    {
        isTyping = true;
        dialogueText.text = ""; // Clear the text

        // Display the text character by character
        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typewriterSpeed);
        }

        isTyping = false;
        continueButton.gameObject.SetActive(true); // Show the continue button
    }

    public void OnContinueClicked()
    {
        if (currentLineIndex < dialogueLines.Length - 1)
        {
            // Move to the next line of dialogue
            currentLineIndex++;
            StartCoroutine(TypeText(dialogueLines[currentLineIndex]));
            continueButton.gameObject.SetActive(false);
        }
        else
        {
            // End of dialogue - load the next scene
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}