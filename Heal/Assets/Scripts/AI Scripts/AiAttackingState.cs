using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAttackingState : AiState
{
    private Transform playerTransform;
    private float attackCheckTimer = 0f;
    private const float DECISION_RATE = 0.2f; // How often to check if we should change states

    public void Enter(AiAgent agent)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // Stop moving when in attack state
        agent.navMeshAgent.isStopped = true;

        // Face the player
        FaceTarget(agent);

        // Set the StateId parameter in the Animator
        if (agent.animator != null)
        {
            agent.animator.SetInteger("StateId", (int)AiStateId.Attacking);
        }

        // Start attack behavior
        if (agent.attackBehavior != null)
        {
            agent.attackBehavior.TriggerAttack();
        }
    }

    public void Exit(AiAgent agent)
    {
        // Resume navigation
        agent.navMeshAgent.isStopped = false;

        // Reset the StateId parameter if needed
        if (agent.animator != null)
        {
            agent.animator.SetInteger("StateId", (int)AiStateId.Idle); // Or another appropriate state
        }
    }

    public AiStateId GetId()
    {
        return AiStateId.Attacking;
    }

    public void Update(AiAgent agent)
    {
        if (playerTransform == null) return;

        // Don't make state decisions during active attack animation
        if (agent.attackBehavior != null && agent.attackBehavior.IsAttacking())
        {
            // Always face the player during attack
            FaceTarget(agent);
            return;
        }

        // Update timer for periodic state checks
        attackCheckTimer += Time.deltaTime;
        if (attackCheckTimer < DECISION_RATE) return;
        attackCheckTimer = 0f;

        // Don't make decisions during game over state
        if (GameOverController.IsGameOver) return;

        // Check distance to player
        float distanceToPlayer = Vector3.Distance(agent.transform.position, playerTransform.position);

        // If player is out of attack range, switch to chase
        if (distanceToPlayer > agent.attackRange)
        {
            agent.stateMachine.ChangeState(AiStateId.Chasing);
            return;
        }

        // Still in range, trigger a new attack
        if (agent.attackBehavior != null)
        {
            FaceTarget(agent);
            agent.attackBehavior.TriggerAttack();
        }
    }

    private void FaceTarget(AiAgent agent)
    {
        if (playerTransform == null) return;

        // Calculate direction to face
        Vector3 direction = playerTransform.position - agent.transform.position;
        direction.y = 0; // Keep on same Y plane

        // Only rotate if we have a valid direction
        if (direction != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            agent.transform.rotation = Quaternion.Slerp(
                agent.transform.rotation,
                lookRotation,
                Time.deltaTime * 5f); // Smooth rotation
        }
    }
}