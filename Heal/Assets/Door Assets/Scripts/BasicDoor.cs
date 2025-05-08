// Modified BasicDoor.cs
using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BasicDoor : MonoBehaviour
{

    public GameObject doorChild;
    public GameObject audioChild;

    public AudioClip openSound;
    public AudioClip closeSound;
    public AudioClip lockedSound;

    public ItemSO requiredKey; // Reference to the required key's ScriptableObject

    private bool inTrigger = false;
    private bool doorOpen = false;
    private bool isLocked = true;

    private MessageDisplay messageDisplay;
    private InventoryManager inventoryManager;

    void Start()
    {
        messageDisplay = FindObjectOfType<MessageDisplay>();
        inventoryManager = FindObjectOfType<InventoryManager>();
    }

    public void doorOpenClose()
    {
        if (doorChild.GetComponent<Animation>().isPlaying == false)
        {
            // Check if player has the required key
            if (isLocked)
            {
                bool hasKey = CheckForKey();
                if (!hasKey)
                {
                    // Play locked sound if available
                    if (lockedSound != null)
                    {
                        audioChild.GetComponent<AudioSource>().clip = lockedSound;
                        audioChild.GetComponent<AudioSource>().Play();
                    }
                    // Display locked message
                    if (messageDisplay != null)
                    {
                        messageDisplay.ShowMessage("I need a key to unlock this door I think");
                    }
                    return;
                }
                else
                {
                    // Key found, unlock the door
                    UnlockDoor();
                }
            }

            // Door is unlocked, proceed with opening/closing
            if (doorOpen == false)
            {
                doorChild.GetComponent<Animation>().Play("Open");
                audioChild.GetComponent<AudioSource>().clip = openSound;
                audioChild.GetComponent<AudioSource>().Play();
                doorOpen = true;
            }
            else
            {
                doorChild.GetComponent<Animation>().Play("Close");
                audioChild.GetComponent<AudioSource>().clip = closeSound;
                audioChild.GetComponent<AudioSource>().Play();
                doorOpen = false;
            }
        }
    }

    private bool CheckForKey()
    {
        if (inventoryManager != null)
        {
            // Check all inventory slots for the required key
            foreach (GameObject slot in inventoryManager.slots)
            {
                ItemSlot itemSlot = slot.GetComponent<ItemSlot>();
                if (itemSlot != null && itemSlot.heldItem != null)
                {
                    InventoryItem inventoryItem = itemSlot.heldItem.GetComponent<InventoryItem>();
                    if (inventoryItem != null && inventoryItem.itemScriptableObject == requiredKey)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void UnlockDoor()
    {
        isLocked = false;
        if (messageDisplay != null)
        {
            messageDisplay.ShowMessage("Door unlocked!");
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.GetComponent<CharacterController>())
            inTrigger = true;
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.GetComponent<CharacterController>())
            inTrigger = false;
    }

    void Update()
    {
        if (inTrigger == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                doorOpenClose();
            }
        }
    }
}