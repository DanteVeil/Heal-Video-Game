using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class InteractToNextScene : MonoBehaviour
{
    [Header("Core Settings")]
    public Camera playerCamera;
    public float interactDistance = 3f;
    public LayerMask interactableLayer;
    public FadeEffect fadeEffect;
    public float sceneTransitionDelay = 1f;

    [Header("Inventory Check")]
    public ItemSO requiredKeyItem;
    public float checkInterval = 0.5f;
    private float nextCheckTime = 0f;
    private bool hasRequiredItem = false;

    [Header("Slot Assignment")]
    [Tooltip("Manually assign the parent GameObject holding all slots (e.g., Canvas/Inventory/Slots)")]
    public GameObject slotsParent; // Assign the Slots parent in Inspector
    private ItemSlot[] cachedSlots;

    [Header("UI Feedback")]
    public GameObject missingItemText;
    public float messageDisplayTime = 2f;
    private float messageHideTime = 0f;

    void Start()
    {
        CacheInventorySlots();
        if (missingItemText) missingItemText.SetActive(false);

        // Initialize fade (optional)
        if (fadeEffect != null)
        {
            fadeEffect.FadeOut(); // Clear screen on start
        }
    }

    void Update()
    {
        CheckInventoryPeriodically();
        HandleMissingItemMessage();
        HandleInteraction();
    }

    private void CacheInventorySlots()
    {
        // If slotsParent is manually assigned, use it
        if (slotsParent != null)
        {
            cachedSlots = slotsParent.GetComponentsInChildren<ItemSlot>(true);
            Debug.Log($"Found {cachedSlots.Length} slots under manually assigned parent.");
            return;
        }

        // Fallback: Try auto-finding the slots parent
        Canvas canvas = FindObjectOfType<Canvas>();
        if (canvas != null)
        {
            Transform inventory = canvas.transform.Find("Inventory");
            if (inventory != null)
            {
                Transform slotsParentTransform = inventory.Find("Slots");
                if (slotsParentTransform != null)
                {
                    cachedSlots = slotsParentTransform.GetComponentsInChildren<ItemSlot>(true);
                    Debug.Log($"Auto-found {cachedSlots.Length} slots under Canvas/Inventory/Slots.");
                }
            }
        }

        // If still no slots, log a warning
        if (cachedSlots == null || cachedSlots.Length == 0)
        {
            Debug.LogWarning("No ItemSlot components found! Inventory checks disabled.");
            hasRequiredItem = true; // Skip checks
        }
    }

    private void CheckInventoryPeriodically()
    {
        if (Time.time >= nextCheckTime && cachedSlots != null)
        {
            hasRequiredItem = CheckForRequiredItem();
            nextCheckTime = Time.time + checkInterval;
        }
    }

    private bool CheckForRequiredItem()
    {
        if (requiredKeyItem == null) return true; // No item required

        foreach (ItemSlot slot in cachedSlots)
        {
            if (slot == null || !slot.heldItem) continue;

            InventoryItem invItem = slot.heldItem.GetComponent<InventoryItem>();
            if (invItem != null && invItem.itemScriptableObject == requiredKeyItem)
                return true;
        }
        return false;
    }

    private void HandleMissingItemMessage()
    {
        if (missingItemText != null && missingItemText.activeSelf && Time.time > messageHideTime)
        {
            missingItemText.SetActive(false);
        }
    }

    private void HandleInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);
            if (Physics.Raycast(ray, out RaycastHit hit, interactDistance, interactableLayer))
            {
                if (hit.collider.gameObject == gameObject)
                {
                    if (hasRequiredItem || requiredKeyItem == null)
                    {
                        StartCoroutine(TransitionToNextScene());
                    }
                    else
                    {
                        ShowMissingItemMessage();
                    }
                }
            }
        }
    }

    private void ShowMissingItemMessage()
    {
        if (missingItemText != null)
        {
            missingItemText.SetActive(true);
            messageHideTime = Time.time + messageDisplayTime;
        }
    }

    IEnumerator TransitionToNextScene()
    {
        // 1. First remove the key from both VISIBLE inventory and PERSISTENT storage
        InventoryManager inventoryManager = FindObjectOfType<InventoryManager>();
        PersistentInventoryManager persistentInventory = FindObjectOfType<PersistentInventoryManager>();

        if (inventoryManager != null)
        {
            // Destroy the key in the visible UI immediately
            ItemSlot[] slots = inventoryManager.GetSlots();
            foreach (ItemSlot slot in slots)
            {
                if (slot.heldItem != null &&
                    slot.heldItem.GetComponent<InventoryItem>()?.itemScriptableObject == requiredKeyItem)
                {
                    Destroy(slot.heldItem);
                    slot.heldItem = null;
                    Debug.Log("Key removed from visible inventory");
                    break; // Only remove one key (in case of duplicates)
                }
            }

            // Update persistent storage
            if (persistentInventory != null)
            {
                persistentInventory.RemoveItem(requiredKeyItem);
                persistentInventory.SaveInventory(slots);
                Debug.Log("Key removed from persistent storage");
            }
        }

        // 2. Visual feedback (fade effect)
        if (fadeEffect != null)
        {
            fadeEffect.FadeIn();
            yield return new WaitForSeconds(fadeEffect.fadeDuration);
        }

        // 3. Brief delay before actual scene transition
        yield return new WaitForSeconds(sceneTransitionDelay);

        // 4. Load next scene
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
        }
        else
        {
            Debug.LogWarning("No next scene available!");
        }
    }
}