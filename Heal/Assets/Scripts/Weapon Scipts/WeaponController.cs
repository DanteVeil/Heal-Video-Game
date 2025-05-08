using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ItemSO gunItem;
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private GunProperties gunProperties;
    [SerializeField] private AmmoSystem ammoSystem;
    [SerializeField] private Transform slotsParent; // Now serializable for manual assignment
    [SerializeField] private InventoryManager inventoryManager; // Add this reference

    [Header("Settings")]
    [SerializeField] private float checkInterval = 0.5f;
    [SerializeField] private LayerMask targetLayers;

    private Animator gunAnimator;
    private bool hasGun = false;
    private bool isGunEquipped = false;
    private float nextCheckTime = 0f;
    private float nextFireTime = 0f;
    private GameObject currentGunModel;
    private AudioSource audioSource;
    private Camera playerCamera;
    private ItemSlot[] cachedSlots; // Cached slots for better performance

    private void Start()
    {
        if (!ValidateReferences()) return;

        SetupWeaponHolder();
        SetupCamera();
        SetupAudio();
        FindAndCacheSlots();
    }

    private bool ValidateReferences()
    {
        if (!gunItem)
        {
            Debug.LogError("Gun ItemSO not assigned!", this);
            enabled = false;
            return false;
        }

        if (!gunProperties)
        {
            Debug.LogError("Gun Properties not assigned!", this);
            enabled = false;
            return false;
        }

        if (!ammoSystem)
        {
            Debug.LogError("AmmoSystem not assigned!", this);
            enabled = false;
            return false;
        }

        // Find inventory manager if not assigned
        if (!inventoryManager)
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
            if (!inventoryManager)
            {
                Debug.LogError("InventoryManager not found!", this);
                enabled = false;
                return false;
            }
        }

        return true;
    }

    private void SetupWeaponHolder()
    {
        if (!weaponHolder)
        {
            GameObject holderObj = new GameObject("WeaponHolder");
            holderObj.transform.SetParent(Camera.main.transform, false);
            holderObj.transform.localPosition = gunProperties.EquippedPosition;
            holderObj.transform.localRotation = Quaternion.Euler(gunProperties.EquippedRotation);
            weaponHolder = holderObj.transform;
            Debug.Log("Created default weapon holder", holderObj);
        }
    }

    private void SetupCamera()
    {
        playerCamera = Camera.main;
        if (!playerCamera)
        {
            Debug.LogError("Could not find main camera!", this);
            enabled = false;
        }
    }

    private void SetupAudio()
    {
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0f;
    }

    private void FindAndCacheSlots()
    {
        // Try to find slots parent if not assigned
        if (slotsParent == null)
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            if (canvas != null)
            {
                Transform inventory = canvas.transform.Find("Inventory");
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

        // Cache all slots once
        cachedSlots = slotsParent.GetComponentsInChildren<ItemSlot>(true);
        Debug.Log($"Found and cached {cachedSlots.Length} inventory slots", this);
    }

    private void Update()
    {
        if (cachedSlots == null || cachedSlots.Length == 0)
        {
            if (Time.time > nextCheckTime + 2f) // Only try every 2 seconds if slots are missing
            {
                FindAndCacheSlots();
                nextCheckTime = Time.time;
            }
            return;
        }

        if (Time.time >= nextCheckTime)
        {
            CheckForGun();
            nextCheckTime = Time.time + checkInterval;
        }

        // Only toggle gun or shoot if inventory is closed
        if (!inventoryManager.IsInventoryOpen())
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ToggleGunEquip();
            }

            if (isGunEquipped && Input.GetMouseButtonDown(0) && Time.time >= nextFireTime)
            {
                Shoot();
                nextFireTime = Time.time + gunProperties.FireRate;
            }
        }
    }

    private void CheckForGun()
    {
        bool found = false;

        foreach (ItemSlot slot in cachedSlots)
        {
            if (slot == null || !slot.heldItem) continue;

            InventoryItem invItem = slot.heldItem.GetComponent<InventoryItem>();
            if (invItem == null) continue;

            if (invItem.itemScriptableObject == gunItem)
            {
                found = true;
                break;
            }
        }

        if (hasGun != found)
        {
            hasGun = found;
            if (!found && isGunEquipped)
            {
                HideGun();
                isGunEquipped = false;
                Debug.Log("Gun removed from inventory - auto-holstered");
            }
            Debug.Log($"Gun {(found ? "found" : "removed")} in inventory");
        }
    }

    private void ToggleGunEquip()
    {
        if (!hasGun)
        {
            Debug.Log("Cannot equip gun - not in inventory");
            return;
        }

        isGunEquipped = !isGunEquipped;

        if (isGunEquipped)
        {
            ShowGun();
            Debug.Log("Gun equipped");
        }
        else
        {
            HideGun();
            Debug.Log("Gun holstered");
        }
    }

    private void ShowGun()
    {
        if (currentGunModel != null)
        {
            Destroy(currentGunModel);
        }

        if (gunProperties.GunModelPrefab != null)
        {
            currentGunModel = Instantiate(gunProperties.GunModelPrefab, weaponHolder);
            currentGunModel.transform.localPosition = Vector3.zero;
            currentGunModel.transform.localRotation = Quaternion.identity;
            gunAnimator = currentGunModel.GetComponent<Animator>();
        }
        else if (gunItem.prefab != null)
        {
            currentGunModel = Instantiate(gunItem.prefab, weaponHolder);
            currentGunModel.transform.localPosition = Vector3.zero;
            currentGunModel.transform.localRotation = Quaternion.identity;
            gunAnimator = currentGunModel.GetComponent<Animator>();
        }
        else
        {
            Debug.LogWarning("No gun model available to display");
            return;
        }

        if (gunAnimator == null)
        {
            Debug.LogWarning("Animator not found on the gun model!");
        }
    }

    private void HideGun()
    {
        if (currentGunModel != null)
        {
            Destroy(currentGunModel);
            currentGunModel = null;
        }
    }

    public bool IsGunEquipped()
    {
        return isGunEquipped;
    }

    private void Shoot()
    {
        if (ammoSystem != null && !ammoSystem.HasAmmo())
        {
            Debug.Log("Out of ammo!");
            return;
        }

        if (gunProperties.ShootSound != null)
        {
            audioSource.PlayOneShot(gunProperties.ShootSound, gunProperties.SoundVolume);
        }

        if (gunProperties.MuzzleFlashPrefab != null)
        {
            Vector3 muzzlePosition = weaponHolder.TransformPoint(gunProperties.MuzzlePosition);
            Quaternion muzzleRotation = weaponHolder.rotation * Quaternion.Euler(gunProperties.MuzzleRotation);
            GameObject muzzleFlash = Instantiate(gunProperties.MuzzleFlashPrefab, muzzlePosition, muzzleRotation);
            muzzleFlash.transform.parent = weaponHolder;
            Destroy(muzzleFlash, 0.1f);
        }

        if (gunAnimator != null)
        {
            gunAnimator.SetTrigger("Recoil");
        }

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, gunProperties.Range, targetLayers))
        {
            Debug.Log($"Hit: {hit.transform.name} at distance {hit.distance}");

            AiHealthSystem aiHealth = hit.transform.GetComponent<AiHealthSystem>();
            if (aiHealth != null)
            {
                aiHealth.TakeDamage((int)gunProperties.Damage);
                Debug.Log($"Damaged AI: {hit.transform.name} for {gunProperties.Damage} damage");
            }
        }

        if (ammoSystem != null)
        {
            ammoSystem.ConsumeBullet();
        }
    }
}