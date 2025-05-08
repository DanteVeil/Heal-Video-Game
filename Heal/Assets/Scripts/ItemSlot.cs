using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public GameObject heldItem;

    public void SetHeldItem(GameObject item)
    {
        if (item == null)
        {
            heldItem = null;
            return;
        }

        heldItem = item;
        heldItem.transform.SetParent(transform);

        // Ensure proper positioning
        RectTransform rectTransform = heldItem.GetComponent<RectTransform>();
        if (rectTransform != null)
        {
            rectTransform.anchoredPosition = Vector2.zero;
            rectTransform.localScale = Vector3.one;
        }
    }

    public void ClearSlot()
    {
        if (heldItem != null)
        {
            Destroy(heldItem);
            heldItem = null;
        }
    }
}