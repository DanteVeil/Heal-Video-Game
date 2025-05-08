using UnityEngine;
using UnityEngine.UI;

public class HealthStatus : MonoBehaviour
{
    [Header("Health Status Images")]
    [SerializeField] private Texture2D fineStatusImage;
    [SerializeField] private Texture2D cautiousStatusImage;
    [SerializeField] private Texture2D dangerStatusImage;

    [Header("Collision Settings")]
    [SerializeField] private string damageObjectTag = "DamageObject";
    [SerializeField] private float invincibilityTime = 1f;
    [SerializeField] private float hitFlashDuration = 0.2f; // How long character flashes when hit

    [Header("Debug Settings")]
    [SerializeField] private bool debugMode = true; // Toggle debug messages in inspector

    [Header("GUI Settings")]
    [SerializeField] private Vector2 windowPosition = new Vector2(20, 20);
    [SerializeField] private Vector2 windowSize = new Vector2(150, 150);

    private bool showStatus = false;
    private int hitCount = 0;
    private Texture2D currentStatusImage;
    private Rect windowRect;
    private bool isInvincible = false;
    private float invincibilityTimer = 0f;
    private Renderer[] renderers;
    private Color[] originalColors;

    // Events
    public delegate void HealthChangedHandler(int hitCount);
    public event HealthChangedHandler OnHealthChanged;

    public delegate void PlayerDeathHandler();
    public event PlayerDeathHandler OnPlayerDeath;

    private void Start()
    {
        if (debugMode) Debug.Log($"[HealthStatus] Initializing health status system on {gameObject.name}");

        // Initialize the window rectangle and starting image
        windowRect = new Rect(windowPosition.x, windowPosition.y, windowSize.x, windowSize.y);
        currentStatusImage = fineStatusImage;

        // Component checks
        if (GetComponent<Collider>() == null)
        {
            Debug.LogError("[HealthStatus] Missing Collider component! Please add a Collider to the player object.");
        }

        // Image checks
        if (fineStatusImage == null || cautiousStatusImage == null || dangerStatusImage == null)
        {
            Debug.LogError("[HealthStatus] One or more status images are not assigned in the inspector!");
        }

        // Store renderer references for hit flash effect
        renderers = GetComponentsInChildren<Renderer>();
        originalColors = new Color[renderers.Length];

        // Store original colors
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
            {
                originalColors[i] = renderers[i].material.color;
            }
        }

        if (debugMode) Debug.Log("[HealthStatus] Initialization complete. Ready for gameplay.");
    }

    private void Update()
    {
        // Toggle status window when Tab is pressed
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            showStatus = !showStatus;
            if (debugMode) Debug.Log($"[HealthStatus] Status window visibility toggled: {showStatus}");
        }

        // Handle invincibility timer
        if (isInvincible)
        {
            invincibilityTimer -= Time.deltaTime;
            if (invincibilityTimer <= 0)
            {
                isInvincible = false;
                if (debugMode) Debug.Log("[HealthStatus] Invincibility period ended");
            }
        }
    }

    private void OnGUI()
    {
        if (showStatus && currentStatusImage != null)
        {
            windowRect = GUI.Window(0, windowRect, DrawStatusWindow, "Health Status");
        }
    }

    private void DrawStatusWindow(int windowID)
    {
        GUI.DrawTexture(new Rect(10, 20, windowRect.width - 20, windowRect.height - 30), currentStatusImage);
        GUI.DragWindow();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (debugMode) Debug.Log($"[HealthStatus] Collision detected with: {collision.gameObject.name}");
        HandleCollision(collision.gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (debugMode) Debug.Log($"[HealthStatus] Trigger detected with: {other.gameObject.name}");
        HandleCollision(other.gameObject);
    }

    private void HandleCollision(GameObject collider)
    {
        if (!isInvincible)
        {
            if (collider.CompareTag(damageObjectTag))
            {
                if (debugMode) Debug.Log($"[HealthStatus] Valid damage object hit: {collider.name}");
                HandleDamage();

                isInvincible = true;
                invincibilityTimer = invincibilityTime;
                if (debugMode) Debug.Log($"[HealthStatus] Invincibility activated for {invincibilityTime} seconds");
            }
            else if (debugMode)
            {
                Debug.Log($"[HealthStatus] Object {collider.name} doesn't have the DamageObject tag ({damageObjectTag})");
            }
        }
        else if (debugMode)
        {
            Debug.Log("[HealthStatus] Hit ignored - Player is currently invincible");
        }
    }

    // New public method that can be called from other scripts with damage amount
    public void TakeDamage(int damageAmount = 1)
    {
        if (isInvincible)
            return;

        // Apply damage
        hitCount += damageAmount;
        if (debugMode) Debug.Log($"[HealthStatus] Damage taken! Hit count: {hitCount}");

        // Visual feedback
        StartCoroutine(HitFlash());

        // Start invincibility
        isInvincible = true;
        invincibilityTimer = invincibilityTime;

        // Update health status UI
        UpdateHealthStatus();

        // Invoke event
        OnHealthChanged?.Invoke(hitCount);

        // Check for death condition
        if (hitCount > 6)
        {
            OnPlayerDeath?.Invoke();
        }
    }

    // Renamed from the original TakeDamage to avoid conflicts
    private void HandleDamage()
    {
        TakeDamage(1); // Now calls the public method with default damage
    }

    private void UpdateHealthStatus()
    {
        string newStatus = "";
        if (hitCount <= 3)
        {
            currentStatusImage = fineStatusImage;
            newStatus = "FINE";
        }
        else if (hitCount <= 6)
        {
            currentStatusImage = cautiousStatusImage;
            newStatus = "CAUTIOUS";
        }
        else
        {
            currentStatusImage = dangerStatusImage;
            newStatus = "DANGER";
        }

        if (debugMode) Debug.Log($"[HealthStatus] Status updated to: {newStatus} (Hit Count: {hitCount})");
    }

    private System.Collections.IEnumerator HitFlash()
    {
        // Change color to red for all renderers
        foreach (Renderer renderer in renderers)
        {
            if (renderer.material.HasProperty("_Color"))
            {
                renderer.material.color = Color.red;
            }
        }

        // Wait for flash duration
        yield return new WaitForSeconds(hitFlashDuration);

        // Restore original colors
        for (int i = 0; i < renderers.Length; i++)
        {
            if (renderers[i].material.HasProperty("_Color"))
            {
                renderers[i].material.color = originalColors[i];
            }
        }
    }

    public void ResetHealth()
    {
        hitCount = 0;
        currentStatusImage = fineStatusImage;
        isInvincible = false;
        invincibilityTimer = 0f;
        if (debugMode) Debug.Log("[HealthStatus] Health status reset to initial state");
    }

    public int GetHitCount()
    {
        return hitCount;
    }

    // Debug methods
    public void ToggleDebug()
    {
        debugMode = !debugMode;
        Debug.Log($"[HealthStatus] Debug mode {(debugMode ? "enabled" : "disabled")}");
    }

    public string GetCurrentStatus()
    {
        if (hitCount <= 3) return "FINE";
        if (hitCount <= 6) return "CAUTIOUS";
        return "DANGER";
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }

    private void OnValidate()
    {
        // Validate inspector values when they're changed
        if (invincibilityTime < 0)
        {
            Debug.LogWarning("[HealthStatus] Invincibility time cannot be negative. Setting to 0.");
            invincibilityTime = 0;
        }
    }
}