using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIAttackBehavior : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private int damageAmount = 1;
    [SerializeField] private float attackRadius = 1.5f; // Size of the attack hitbox
    [SerializeField] private Transform attackPoint; // Position where the attack originates
    [SerializeField] private LayerMask playerLayer; // Layer for the player
    [SerializeField] private string playerTag = "Player"; // Alternative to layer-based detection

    [Header("Animation")]
    [SerializeField] private string attackTriggerName = "Attack";
    [SerializeField] private float damageDelay = 0.6f; // Time in seconds from animation start to damage point

    // Audio settings (optional)
    [Header("Audio")]
    [SerializeField] private AudioClip attackSound;
    [SerializeField] private AudioSource audioSource;

    [Header("Debug Settings")]
    [SerializeField] private bool debugMode = false;
    [SerializeField] private bool showHitbox = true;

    // Internal variables
    private bool canAttack = true;
    private Animator animator;
    private bool isAttacking = false;

    private void Start()
    {
        animator = GetComponent<Animator>();

        // Initialize attack point if not set
        if (attackPoint == null)
        {
            attackPoint = transform;
            if (debugMode) Debug.LogWarning("Attack point not set - using enemy transform instead");
        }

        // Initialize audio source if needed
        if (audioSource == null && attackSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Verify player layer is set
        if (playerLayer.value == 0)
        {
            if (debugMode) Debug.LogWarning("Player layer not set! Will use tag-based detection only.");
        }
    }

    public void TriggerAttack()
    {
        if (canAttack && !isAttacking)
        {
            StartCoroutine(AttackSequence());
        }
    }

    private IEnumerator AttackSequence()
    {
        // Start attack
        isAttacking = true;
        canAttack = false;

        if (debugMode) Debug.Log("AI starting attack sequence");

        // Trigger animation
        if (animator != null)
        {
            animator.SetTrigger(attackTriggerName);
        }
        else if (debugMode)
        {
            Debug.LogWarning("No animator component found!");
        }

        // Wait for the damage point in animation
        yield return new WaitForSeconds(damageDelay);

        // Apply damage at the right moment in the animation
        ApplyDamage();

        // Play sound effect
        if (audioSource != null && attackSound != null)
        {
            audioSource.PlayOneShot(attackSound);
        }

        // Wait for cooldown
        yield return new WaitForSeconds(attackCooldown - damageDelay);

        if (debugMode) Debug.Log("AI attack cooldown complete");
        canAttack = true;
        isAttacking = false;
    }

    private void ApplyDamage()
    {
        if (debugMode) Debug.Log("Checking for player in attack range");

        // Check if  hit player using a sphere cast
        bool playerHit = false;


        if (playerLayer.value != 0)
        {
            Collider[] hitColliders = Physics.OverlapSphere(attackPoint.position, attackRadius, playerLayer);

            foreach (Collider hit in hitColliders)
            {
                // Check if hit the player
                HealthStatus playerHealth = hit.GetComponent<HealthStatus>();
                if (playerHealth != null && !playerHealth.IsInvincible())
                {
                    // Apply damage
                    playerHealth.TakeDamage(damageAmount);
                    playerHit = true;

                    if (debugMode) Debug.Log("Player hit by enemy attack!");
                }
            }
        }


        if (!playerHit)
        {
            Collider[] allColliders = Physics.OverlapSphere(attackPoint.position, attackRadius);

            foreach (Collider hit in allColliders)
            {
                if (hit.CompareTag(playerTag))
                {
                    HealthStatus playerHealth = hit.GetComponent<HealthStatus>();
                    if (playerHealth != null && !playerHealth.IsInvincible())
                    {
                        // Apply damage
                        playerHealth.TakeDamage(damageAmount);
                        playerHit = true;

                        if (debugMode) Debug.Log("Player hit by enemy attack (tag detection)!");
                    }
                }
            }
        }

        if (!playerHit && debugMode)
        {
            Debug.Log("Attack missed - no player in range at damage point");
        }
    }

    // Utility methods
    public float GetAttackCooldown()
    {
        return attackCooldown;
    }

    public float GetAttackRange()
    {
        return attackRadius;
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    // Visual debugging
    private void OnDrawGizmosSelected()
    {
        if (!showHitbox) return;

        if (attackPoint == null)
            attackPoint = transform;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}