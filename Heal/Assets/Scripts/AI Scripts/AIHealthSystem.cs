using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiHealthSystem : MonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 3;
    private int currentHealth;

    [Header("Debug Settings")]
    [SerializeField] private bool debugMode = true;

    private AiAgent aiAgent;
    private bool isDead = false;

    // Reference to the player's AmmoSystem
    private AmmoSystem playerAmmoSystem;

    void Start()
    {
        currentHealth = maxHealth;
        aiAgent = GetComponent<AiAgent>();

        if (aiAgent == null)
        {
            Debug.LogError("[AiHealthSystem] Missing AiAgent component!");
            enabled = false;
            return;
        }

        // Find the player's AmmoSystem component
        playerAmmoSystem = GameObject.FindGameObjectWithTag("Player").GetComponent<AmmoSystem>();
        if (playerAmmoSystem == null)
        {
            Debug.LogError("[AiHealthSystem] Player AmmoSystem not found!");
            enabled = false;
            return;
        }

        if (debugMode) Debug.Log($"[AiHealthSystem] Initialized with {currentHealth} health points");
    }

    void Update()
    {
        // Check for player mouse click on the AI
        if (Input.GetMouseButtonDown(0))
        {
            CheckForPlayerAttack();
        }
    }

    private void CheckForPlayerAttack()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.transform == this.transform)
            {
                // Check if the player has ammo before applying damage
                if (playerAmmoSystem != null && playerAmmoSystem.HasAmmo())
                {
                    TakeDamage(1);
                    if (debugMode) Debug.Log($"[AiHealthSystem] AI was clicked and took damage. Health: {currentHealth}");
                }
                else
                {
                    if (debugMode) Debug.Log("[AiHealthSystem] Player has no ammo. AI did not take damage.");
                }
            }
        }
    }

    public void TakeDamage(int damageAmount)
    {
        if (isDead) return;

        currentHealth -= damageAmount;

        if (currentHealth <= 0)
        {
            Die();
        }
        else
        {
            // You could trigger hurt animation or effects here
            if (debugMode) Debug.Log($"[AiHealthSystem] AI took damage. Health: {currentHealth}");
        }
    }

    private void Die()
    {
        isDead = true;
        currentHealth = 0;

        if (debugMode) Debug.Log("[AiHealthSystem] AI has died");

        // Notify the state machine to change to death state
        aiAgent.stateMachine.ChangeState(AiStateId.Death);
    }

    public int GetCurrentHealth()
    {
        return currentHealth;
    }

    public bool IsDead()
    {
        return isDead;
    }

    public void ResetHealth()
    {
        currentHealth = maxHealth;
        isDead = false;
        if (debugMode) Debug.Log("[AiHealthSystem] Health reset to maximum");
    }
}