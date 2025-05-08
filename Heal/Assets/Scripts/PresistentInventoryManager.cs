using UnityEngine;

public class PersistentInventoryManager : MonoBehaviour
{
    public static PersistentInventoryManager Instance { get; private set; }

    // Store inventory data
    public ItemSO[] savedItems = new ItemSO[8];

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SaveInventory(ItemSlot[] slots)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].heldItem != null)
            {
                savedItems[i] = slots[i].heldItem.GetComponent<InventoryItem>().itemScriptableObject;
            }
            else
            {
                savedItems[i] = null;
            }
        }
    }

    public void LoadInventory(ItemSlot[] slots, GameObject itemPrefab)
    {
        for (int i = 0; i < savedItems.Length; i++)
        {
            if (savedItems[i] != null)
            {
                GameObject newItem = Instantiate(itemPrefab, slots[i].transform);
                InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
                inventoryItem.itemScriptableObject = savedItems[i];
                slots[i].SetHeldItem(newItem);
            }
        }
    }
    public void RemoveItem(ItemSO itemToRemove)
    {
        for (int i = 0; i < savedItems.Length; i++)
        {
            if (savedItems[i] == itemToRemove)
            {
                savedItems[i] = null; // Remove the key from persistent storage
                break;
            }
        }
    }
}