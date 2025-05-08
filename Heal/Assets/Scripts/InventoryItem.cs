using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemSO itemScriptableObject;
    [SerializeField] Image iconImage;

    private void Awake()
    {
        // If iconImage reference is missing, try to find it
        if (iconImage == null)
        {
            iconImage = GetComponent<Image>();
            if (iconImage == null)
            {
                iconImage = GetComponentInChildren<Image>();
                if (iconImage == null)
                {
                    Debug.LogError("No Image component found for inventory item");
                }
            }
        }
    }

    private void Start()
    {
        UpdateIcon();
    }

    private void OnEnable()
    {
        UpdateIcon();
    }

    private void UpdateIcon()
    {
        if (itemScriptableObject != null && iconImage != null)
        {
            iconImage.sprite = itemScriptableObject.icon;
            iconImage.enabled = true;
        }
        else if (iconImage != null)
        {
            iconImage.enabled = false;
        }
    }
}