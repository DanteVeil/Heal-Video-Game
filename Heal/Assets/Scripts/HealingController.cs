using UnityEngine;

public class HealingController : MonoBehaviour
{
    [SerializeField] private ItemSO healingItem;
    [SerializeField] private int healAmount = 3;

    private Canvas mainCanvas;
    [SerializeField] private Transform slotsParent;
    private HealthStatus playerHealth;

    private void Start()
    {
        if (!healingItem)
        {
            Debug.LogError("Healing ItemSO not assigned!");
            enabled = false;
            return;
        }

        playerHealth = GetComponent<HealthStatus>();
        if (!playerHealth)
        {
            Debug.LogError("HealthStatus component not found on player!");
            enabled = false;
            return;
        }

        mainCanvas = GameObject.FindObjectOfType<Canvas>();
        if (mainCanvas)
        {
            Transform inventory = mainCanvas.transform.Find("Inventory");
            if (inventory)
            {
                slotsParent = inventory.Find("Slots");
            }
        }

        if (!slotsParent)
        {
            Debug.LogError("Could not find Slots container!");
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            CheckAndHeal();
        }
    }

    private void CheckAndHeal()
    {
        int currentHits = playerHealth.GetHitCount();
        if (currentHits == 0)
        {
            Debug.Log("Player is already at full health!");
            return;
        }

        foreach (Transform slotTransform in slotsParent)
        {
            ItemSlot slot = slotTransform.GetComponent<ItemSlot>();
            if (!slot || !slot.heldItem) continue;

            InventoryItem invItem = slot.heldItem.GetComponent<InventoryItem>();
            if (!invItem) continue;

            if (invItem.itemScriptableObject == healingItem)
            {
                // Calculate healing
                int healsNeeded = Mathf.Min(healAmount, currentHits);
                int newHitCount = currentHits - healsNeeded;

                // Reset hit count and destroy healing item
                for (int i = currentHits; i > newHitCount; i--)
                {
                    playerHealth.ResetHealth();
                }

                Destroy(slot.heldItem);
                slot.heldItem = null;

                Debug.Log($"Used healing item! Recovered {healsNeeded} hit points.");
                return;
            }
        }

        Debug.Log("No healing item found in inventory");
    }
}