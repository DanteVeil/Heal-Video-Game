using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    // Array for the inventory slots
    public GameObject[] slots = new GameObject[8];
    [SerializeField] GameObject inventoryParent;
    [SerializeField] GameObject itemPrefab;
    [SerializeField] private PersistentInventoryManager persistentInventory;

    private GameObject draggedObject;
    private GameObject lastItemSlot;
    private Canvas inventoryCanvas;
    private bool isDragging = false;

    private bool isInventoryOpened;

    private void Awake()
    {
        // Get the canvas component for proper drag handling
        inventoryCanvas = GetComponentInParent<Canvas>();
        if (inventoryCanvas == null)
        {
            Debug.LogError("Inventory canvas not found!");
        }
    }

    private void Start()
    {
        InitializePersistentInventory();
        InitializeSlots();
    }

    private void InitializePersistentInventory()
    {
        if (persistentInventory == null)
        {
            persistentInventory = FindObjectOfType<PersistentInventoryManager>();
            if (persistentInventory == null)
            {
                GameObject obj = new GameObject("PersistentInventory");
                persistentInventory = obj.AddComponent<PersistentInventoryManager>();
                DontDestroyOnLoad(obj);
            }
        }

        // Load saved inventory
        persistentInventory.LoadInventory(GetSlots(), itemPrefab);
    }

    private void InitializeSlots()
    {
        // Add event triggers to each slot
        foreach (GameObject slot in slots)
        {
            if (slot != null)
            {
                EventTrigger trigger = slot.GetComponent<EventTrigger>();
                if (trigger == null)
                {
                    trigger = slot.AddComponent<EventTrigger>();
                }

                // Clear existing triggers to avoid duplicates
                trigger.triggers.Clear();

                // Add pointer down event
                EventTrigger.Entry pointerDown = new EventTrigger.Entry();
                pointerDown.eventID = EventTriggerType.PointerDown;
                pointerDown.callback.AddListener((data) => { OnPointerDown((PointerEventData)data, slot); });
                trigger.triggers.Add(pointerDown);

                // Add drag event
                EventTrigger.Entry drag = new EventTrigger.Entry();
                drag.eventID = EventTriggerType.Drag;
                drag.callback.AddListener((data) => { OnDrag((PointerEventData)data); });
                trigger.triggers.Add(drag);

                // Add pointer up event
                EventTrigger.Entry pointerUp = new EventTrigger.Entry();
                pointerUp.eventID = EventTriggerType.PointerUp;
                pointerUp.callback.AddListener((data) => { OnPointerUp((PointerEventData)data); });
                trigger.triggers.Add(pointerUp);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Toggle inventory visibility
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Don't open inventory if pause menu is open
            if (PauseMenu.IsGamePaused) return;

            isInventoryOpened = !isInventoryOpened;
            inventoryParent.SetActive(isInventoryOpened);
            UnityEngine.Cursor.lockState = isInventoryOpened ? CursorLockMode.None : CursorLockMode.Locked;
            UnityEngine.Cursor.visible = isInventoryOpened;
        }

        // Save inventory before scene changes
        if (persistentInventory != null && SceneManager.GetActiveScene().isLoaded)
        {
            persistentInventory.SaveInventory(GetSlots());
        }
    }

    public bool IsInventoryOpen()
    {
        return isInventoryOpened;
    }

    public ItemSlot[] GetSlots()
    {
        ItemSlot[] itemSlots = new ItemSlot[slots.Length];
        for (int i = 0; i < slots.Length; i++)
        {
            itemSlots[i] = slots[i].GetComponent<ItemSlot>();
        }
        return itemSlots;
    }

    public void OnPointerDown(PointerEventData eventData, GameObject clickedSlot)
    {
        if (!isInventoryOpened) return;
        if (eventData.button != PointerEventData.InputButton.Left) return;

        ItemSlot slot = clickedSlot.GetComponent<ItemSlot>();
        if (slot != null && slot.heldItem != null)
        {
            draggedObject = slot.heldItem;
            draggedObject.transform.SetParent(inventoryParent.transform); // Move to parent container while dragging
            draggedObject.transform.SetAsLastSibling(); // Ensure it's drawn on top
            lastItemSlot = clickedSlot;
            slot.heldItem = null;
            isDragging = true;

            Debug.Log("Started dragging item from slot");
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isInventoryOpened || !isDragging || draggedObject == null) return;

        // Update position to follow mouse
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            inventoryParent.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPoint);

        draggedObject.GetComponent<RectTransform>().localPosition = localPoint;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isInventoryOpened || !isDragging || draggedObject == null) return;
        if (eventData.button != PointerEventData.InputButton.Left) return;

        isDragging = false;
        GameObject targetSlot = null;

        // Check if  on a valid slot
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            ItemSlot slot = result.gameObject.GetComponent<ItemSlot>();
            if (slot != null)
            {
                targetSlot = result.gameObject;
                break;
            }
        }

        // Check if dropping outside inventory
        if (!IsPointerOverInventory(eventData))
        {
            // Simply destroy the item when dropped outside inventory
            Debug.Log("Item dropped outside inventory - destroying item");
            Destroy(draggedObject);
            draggedObject = null;
            // The item is already removed from its slot when we started dragging
        }
        else if (targetSlot == null)
        {
            // Dropping in inventory but not on a slot, return to original slot
            ReturnToLastSlot();
        }
        else
        {
            // Process item swap or placement in new slot
            ItemSlot targetItemSlot = targetSlot.GetComponent<ItemSlot>();

            if (targetItemSlot.heldItem == null)
            {
                // Empty slot - place item
                targetItemSlot.SetHeldItem(draggedObject);
                Debug.Log("Item placed in empty slot");
            }
            else
            {
                // Occupied slot - swap items
                GameObject tempItem = targetItemSlot.heldItem;
                targetItemSlot.heldItem = null;
                targetItemSlot.SetHeldItem(draggedObject);

                ItemSlot lastSlot = lastItemSlot.GetComponent<ItemSlot>();
                lastSlot.SetHeldItem(tempItem);
                Debug.Log("Items swapped between slots");
            }
        }

        draggedObject = null;
    }

    private void ReturnToLastSlot()
    {
        if (lastItemSlot != null)
        {
            ItemSlot slot = lastItemSlot.GetComponent<ItemSlot>();
            if (slot != null)
            {
                slot.SetHeldItem(draggedObject);
                Debug.Log("Item returned to original slot");
            }
        }
        else
        {
            // Fallback: find first empty slot
            for (int i = 0; i < slots.Length; i++)
            {
                ItemSlot slot = slots[i].GetComponent<ItemSlot>();
                if (slot != null && slot.heldItem == null)
                {
                    slot.SetHeldItem(draggedObject);
                    Debug.Log($"Item placed in fallback empty slot {i}");
                    break;
                }
            }
        }
    }

    private bool IsPointerOverInventory(PointerEventData eventData)
    {
        // Check if the pointer is over any inventory UI element
        var results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        foreach (var result in results)
        {
            if (result.gameObject.transform.IsChildOf(inventoryParent.transform))
            {
                return true;
            }
        }
        return false;
    }

    public void ItemPicked(GameObject pickedItem)
    {
        Debug.Log("ItemPicked method called.");
        GameObject emptySlot = null;

        // Find an empty slot
        for (int i = 0; i < slots.Length; i++)
        {
            ItemSlot slot = slots[i].GetComponent<ItemSlot>();
            if (slot != null && slot.heldItem == null)
            {
                Debug.Log($"Empty slot found at index: {i}");
                emptySlot = slots[i];
                break;
            }
        }

        if (emptySlot != null)
        {
            ItemPickable pickableComponent = pickedItem.GetComponent<ItemPickable>();
            if (pickableComponent != null && pickableComponent.itemScriptableObject != null)
            {
                ItemSO itemSO = pickableComponent.itemScriptableObject;
                GameObject newInventoryItem = Instantiate(itemPrefab, emptySlot.transform);
                InventoryItem inventoryItem = newInventoryItem.GetComponent<InventoryItem>();

                if (inventoryItem != null)
                {
                    inventoryItem.itemScriptableObject = itemSO;
                    Debug.Log($"Added item: {itemSO.name}");
                }

                emptySlot.GetComponent<ItemSlot>().SetHeldItem(newInventoryItem);
                Destroy(pickedItem);
            }
            else
            {
                Debug.LogError("Picked item does not have a valid ItemSO assigned.");
            }
        }
        else
        {
            Debug.Log("No empty slot available.");
        }
    }
}