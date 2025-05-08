using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerItemInteract : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] private InventoryManager inventoryManager;
    [SerializeField] private AmmoSystem ammoSystem;

    // Start is called before the first frame update
    void Start()
    {
        if (!cam)
        {
            Debug.LogError("Camera not assigned!");
            enabled = false;
            return;
        }

        if (!inventoryManager)
        {
            Debug.LogError("InventoryManager not assigned!");
            enabled = false;
            return;
        }

        if (!ammoSystem)
        {
            Debug.LogError("AmmoSystem not assigned!");
            enabled = false;
            return;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;
            Debug.Log("E Used");

            if (Physics.Raycast(ray, out hitInfo, 3))
            {
                ItemPickable item = hitInfo.collider.gameObject.GetComponent<ItemPickable>();

                if (item != null)
                {
                    Debug.Log($"Interacting with: {item.name}");
                    Debug.Log($"ItemSO: {item.itemScriptableObject.name}");

                    inventoryManager.ItemPicked(hitInfo.collider.gameObject);

                    if (item.itemScriptableObject == ammoSystem.AmmoItemSO)
                    {
                        ammoSystem.AddAmmo();
                    }

                    Debug.Log($"{hitInfo.collider.gameObject.name} was picked");
                }
                else
                {
                    Debug.Log("No ItemPickable component detected on the hit object.");
                }
            }
        }
    }
}