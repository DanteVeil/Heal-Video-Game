using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Collider))]
public class InteractToActivate : MonoBehaviour
{
    [Header("Interaction Settings")]
    public Camera playerCamera;
    public float interactDistance = 3f;
    public LayerMask interactableLayer;

    [Header("Object to Spawn")]
    [Tooltip("The prefab or scene object to activate")]
    public GameObject objectToActivate;

    [Header("Spawn Settings")]
    public Transform spawnPoint;

    [Header("UI Settings")]
    [Tooltip("The UI element showing interaction prompt")]
    public GameObject interactPrompt;

    [Header("Sound Settings")]
    [Tooltip("Play sound when interacted with")]
    public bool playSoundOnInteract = false;

    private bool hasActivated = false;
    private SoundPlayerOnInteract soundPlayer;

    void Start()
    {
        // Validate fields
        if (playerCamera == null) playerCamera = Camera.main;
        if (interactPrompt != null) interactPrompt.SetActive(false);

        if (objectToActivate == null)
        {
            Debug.LogError("No object to activate assigned!", this);
            enabled = false;
            return;
        }

        // Get sound player component if sound should be played
        if (playSoundOnInteract)
        {
            soundPlayer = GetComponent<SoundPlayerOnInteract>();
            if (soundPlayer == null)
            {
                Debug.LogWarning("Sound on interact is enabled but no SoundPlayerOnInteract component found!", this);
            }
        }
    }

    void Update()
    {
        if (hasActivated) return;

        bool isLooking = IsLookingAtThisObject();

        // Update prompt visibility
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(isLooking);
        }

        // Handle interaction
        if (isLooking && Input.GetKeyDown(KeyCode.E))
        {
            ActivateObject();
        }
    }

    bool IsLookingAtThisObject()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
        return Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayer)
               && hit.collider.gameObject == this.gameObject;
    }

    void ActivateObject()
    {
        // Activate or instantiate the correct object
        GameObject activatedObject;
        if (objectToActivate.scene.rootCount == 0) // Is prefab
        {
            activatedObject = Instantiate(objectToActivate);
        }
        else // Is scene object
        {
            activatedObject = objectToActivate;
            activatedObject.SetActive(true);
        }

        // Position the object
        if (spawnPoint != null)
        {
            activatedObject.transform.SetPositionAndRotation(spawnPoint.position, spawnPoint.rotation);
        }

        hasActivated = true;

        // Hide prompt
        if (interactPrompt != null)
        {
            interactPrompt.SetActive(false);
        }

        // Play sound if enabled
        if (playSoundOnInteract && soundPlayer != null)
        {
            soundPlayer.ActivateSound();
        }

        Debug.Log($"Activated: {activatedObject.name}", activatedObject);
    }

    void OnDrawGizmosSelected()
    {
        if (playerCamera != null)
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(playerCamera.transform.position,
                           playerCamera.transform.position + playerCamera.transform.forward * interactDistance);
        }
    }
}