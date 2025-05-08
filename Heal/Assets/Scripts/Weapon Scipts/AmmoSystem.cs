using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AmmoSystem : MonoBehaviour
{
    [Header("Ammo Settings")]
    [SerializeField] private ItemSO ammoItemSO; // The ScriptableObject representing the ammo box
    [SerializeField] private int bulletsPerAmmoBox = 10; // Each ammo box contains 10 bullets

    [Header("UI Settings")]
    [SerializeField] private Text ammoText; // UI Text to display the ammo count in the lower right corner

    private int currentBullets = 0; // Current number of bullets available
    private bool isGunEquipped = false; // Track if the gun is currently equipped

    private Transform slotsParent; // Reference to the inventory slots parent
    private WeaponController weaponController; // Reference to the WeaponController to check if the gun is equipped
    private PersistentAmmoManager persistentAmmoManager;
    private bool initializedAmmo = false;

    // Public property to access the ammoItemSO
    public ItemSO AmmoItemSO => ammoItemSO;

    private void Awake()
    {
        // Make sure PersistentAmmoManager exists
        persistentAmmoManager = PersistentAmmoManager.Instance;

        // Subscribe to scene loaded event to save ammo before scene changes
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        // Unsubscribe to prevent memory leaks
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset the initialization flag when a new scene is loaded
        initializedAmmo = false;
    }

    private void Start()
    {
        // Find the inventory slots parent in the hierarchy
        Canvas mainCanvas = GameObject.FindObjectOfType<Canvas>();
        if (mainCanvas)
        {
            Transform inventory = mainCanvas.transform.Find("Inventory");
            if (inventory)
            {
                slotsParent = inventory.Find("Slots");
                if (!slotsParent)
                {
                    Debug.LogError("Could not find Slots container in the inventory!");
                }
            }
        }

        // Get the WeaponController component to check if the gun is equipped
        weaponController = GetComponent<WeaponController>();
        if (!weaponController)
        {
            Debug.LogError("WeaponController component not found on the player!");
        }

        // Initialize the ammo text
        UpdateAmmoText();
    }

    private void Update()
    {
        // Initialize ammo count from persistent storage if we haven't done so yet
        if (!initializedAmmo)
        {
            InitializeAmmoFromPersistentStorage();
        }

        // Check if the gun is equipped (pressing "1" toggles the gun)
        if (weaponController != null)
        {
            isGunEquipped = weaponController.IsGunEquipped();
        }

        // If the gun is equipped, update the ammo count and display it
        if (isGunEquipped)
        {
            UpdateAmmoText();
        }
        else
        {
            // Hide the ammo text when the gun is holstered
            if (ammoText != null)
            {
                ammoText.text = "";
            }
        }

        // Save ammo count when changing scenes
        if (persistentAmmoManager != null)
        {
            persistentAmmoManager.SaveAmmoCount(currentBullets);
        }
    }

    private void InitializeAmmoFromPersistentStorage()
    {
        if (persistentAmmoManager != null)
        {
            // Only load ammo if we have ammo boxes in inventory
            if (HasAmmoBoxInInventory())
            {
                // Set current bullets to the value saved in the persistent manager
                currentBullets = persistentAmmoManager.GetSavedAmmoCount();
                Debug.Log($"Loaded {currentBullets} bullets from persistent storage");
            }
            initializedAmmo = true;
        }
    }

    private bool HasAmmoBoxInInventory()
    {
        if (slotsParent == null || ammoItemSO == null) return false;

        // Check if there's at least one ammo box in the inventory
        foreach (Transform slotTransform in slotsParent)
        {
            ItemSlot slot = slotTransform.GetComponent<ItemSlot>();
            if (!slot || !slot.heldItem) continue;

            InventoryItem invItem = slot.heldItem.GetComponent<InventoryItem>();
            if (!invItem) continue;

            if (invItem.itemScriptableObject == ammoItemSO)
            {
                return true;
            }
        }
        return false;
    }

    // Public method to check if there are bullets available
    public bool HasAmmo()
    {
        return currentBullets > 0;
    }

    // Public method to consume one bullet when firing
    public void ConsumeBullet()
    {
        if (currentBullets > 0)
        {
            currentBullets--;
            UpdateAmmoText();


            if (currentBullets == 0)
            {
                RemoveAmmoPackFromInventory();
            }


            if (persistentAmmoManager != null)
            {
                persistentAmmoManager.SaveAmmoCount(currentBullets);
            }
        }
    }

    //Public method to add ammo when picking up an ammo box
    public void AddAmmo()
    {
        currentBullets += bulletsPerAmmoBox;
        UpdateAmmoText();

        // Update persistent storage
        if (persistentAmmoManager != null)
        {
            persistentAmmoManager.SaveAmmoCount(currentBullets);
        }
    }

    private void RemoveAmmoPackFromInventory()
    {
        if (slotsParent == null || ammoItemSO == null)
        {
            Debug.LogError("Slots parent or ammo ScriptableObject not assigned!");
            return;
        }

        // Find and remove the first ammo pack in the inventory
        foreach (Transform slotTransform in slotsParent)
        {
            ItemSlot slot = slotTransform.GetComponent<ItemSlot>();
            if (!slot || !slot.heldItem) continue;

            InventoryItem invItem = slot.heldItem.GetComponent<InventoryItem>();
            if (!invItem) continue;

            if (invItem.itemScriptableObject == ammoItemSO)
            {
                Destroy(slot.heldItem); 
                slot.heldItem = null;
                Debug.Log("Ammo pack removed from inventory.");
                break;
            }
        }
    }

    private void UpdateAmmoText()
    {
        if (ammoText != null)
        {
            ammoText.text = $"Ammo: {currentBullets}"; 
        }
    }
}