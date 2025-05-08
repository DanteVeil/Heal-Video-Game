using UnityEngine;
using UnityEngine.UI;

public class InteractableNote : MonoBehaviour
{
    [Header("Note Settings")]
    public GameObject noteCanvas; 
    public KeyCode interactKey = KeyCode.E; 
    public float interactionRange = 2f; 

    private bool isNoteVisible = false;
    private Transform player;

    void Start()
    { 
        player = GameObject.FindGameObjectWithTag("Player").transform;

        // Hide the note UI at the start
        if (noteCanvas != null)
        {
            noteCanvas.SetActive(false);
        }
    }

    void Update()
    {
        // Checks if the player is within interaction range
        if (Vector3.Distance(transform.position, player.position) <= interactionRange)
        {
            // Checks for interaction key press
            if (Input.GetKeyDown(interactKey))
            {
                ToggleNote();
            }
        }
        else if (isNoteVisible)
        {
            // Hides the note if the player moves out of range
            HideNote();
        }
    }

    void ToggleNote()
    {
        isNoteVisible = !isNoteVisible;
        noteCanvas.SetActive(isNoteVisible);

        Time.timeScale = isNoteVisible ? 0f : 1f;
    }

    void HideNote()
    {
        isNoteVisible = false;
        noteCanvas.SetActive(false);

        // Resume the game
        Time.timeScale = 1f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRange);
    }
}