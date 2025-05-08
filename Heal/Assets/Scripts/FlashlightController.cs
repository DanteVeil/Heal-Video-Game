using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class FlashlightController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ItemSO flashlightItem;
    [SerializeField] private Light spotLight;
    [SerializeField] private Transform slotsParent; // Assign manually in inspector for best performance

    [Header("Light Settings")]
    [SerializeField] private float lightIntensity = 2f;
    [SerializeField] private float lightRange = 10f;
    [SerializeField] private float lightAngle = 45f;
    [SerializeField] private float checkInterval = 0.5f;

    private bool hasFlashlight = false;
    private bool isFlashlightOn = false;
    private float nextCheckTime = 0f;
    private ItemSlot[] inventorySlots; // Cached slots for better performance

#if UNITY_EDITOR
    [SerializeField, ReadOnly] private Transform debugSlotsParent;
#endif

    private void Awake()
    {
        InitializeFlashlightSystem();
    }

    private void InitializeFlashlightSystem()
    {
        if (!flashlightItem)
        {
            Debug.LogError("Flashlight ItemSO not assigned!", this);
            enabled = false;
            return;
        }

        FindAndCacheSlots();
        SetupSpotlight();
    }

    private void FindAndCacheSlots()
    {
        // Try direct assignment first
        if (slotsParent == null)
        {
            GameObject canvas = GameObject.Find("Canvas");
            if (canvas != null)
            {
                Transform inventory = canvas.transform.Find("Inventory");
                if (inventory != null)
                {
                    slotsParent = inventory.Find("Slots");
                }
            }
        }

        // Fallback to more extensive search if needed
        if (slotsParent == null)
        {
            Canvas mainCanvas = GameObject.FindObjectOfType<Canvas>(true);
            if (mainCanvas != null)
            {
                Transform inventory = mainCanvas.transform.Find("Inventory");
                if (inventory != null)
                {
                    slotsParent = inventory.Find("Slots");
                }
            }
        }

        if (slotsParent == null)
        {
            Debug.LogError("Could not find Slots container! Expected path: Canvas/Inventory/Slots", this);
            enabled = false;
            return;
        }

#if UNITY_EDITOR
        debugSlotsParent = slotsParent;
#endif

        // Cache all slots once
        inventorySlots = slotsParent.GetComponentsInChildren<ItemSlot>(true);
        Debug.Log($"Cached {inventorySlots.Length} inventory slots", this);
    }

    private void SetupSpotlight()
    {
        if (spotLight == null)
        {
            GameObject lightObj = new GameObject("FlashlightBeam");
            lightObj.transform.SetParent(Camera.main.transform, false);
            lightObj.transform.localPosition = Vector3.zero;
            lightObj.transform.localRotation = Quaternion.identity;

            spotLight = lightObj.AddComponent<Light>();
            spotLight.type = LightType.Spot;
            spotLight.range = lightRange;
            spotLight.spotAngle = lightAngle;
            spotLight.intensity = lightIntensity;
            spotLight.enabled = false;
        }

        Debug.Log("Spotlight setup complete", spotLight);
    }

    private void Update()
    {
        if (inventorySlots == null || inventorySlots.Length == 0)
        {
            if (Time.time > nextCheckTime)
            {
                FindAndCacheSlots();
                nextCheckTime = Time.time + 2f; // Only try every 2 seconds if missing
            }
            return;
        }

        if (Time.time >= nextCheckTime)
        {
            CheckForFlashlight();
            nextCheckTime = Time.time + checkInterval;
        }

        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleFlashlight();
        }
    }

    private void CheckForFlashlight()
    {
        bool found = false;

        foreach (ItemSlot slot in inventorySlots)
        {
            if (slot == null || !slot.heldItem) continue;

            InventoryItem invItem = slot.heldItem.GetComponent<InventoryItem>();
            if (invItem == null) continue;

            if (invItem.itemScriptableObject == flashlightItem)
            {
                found = true;
                break;
            }
        }

        if (hasFlashlight != found)
        {
            hasFlashlight = found;
            if (!found) TurnOffFlashlight();
            Debug.Log($"Flashlight {(found ? "found" : "removed")} in inventory");
        }
    }

    private void ToggleFlashlight()
    {
        if (!hasFlashlight) return;

        isFlashlightOn = !isFlashlightOn;
        spotLight.enabled = isFlashlightOn;
        Debug.Log($"Flashlight toggled: {(isFlashlightOn ? "ON" : "OFF")}");
    }

    private void TurnOffFlashlight()
    {
        if (isFlashlightOn)
        {
            isFlashlightOn = false;
            spotLight.enabled = false;
            Debug.Log("Flashlight automatically turned off");
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        debugSlotsParent = slotsParent;
    }
#endif
}

#if UNITY_EDITOR
[CustomPropertyDrawer(typeof(ReadOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label);
        GUI.enabled = true;
    }
}

public class ReadOnlyAttribute : PropertyAttribute { }
#endif