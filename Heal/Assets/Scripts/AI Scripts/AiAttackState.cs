/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiAttackState : AiState
{
    private Transform playerTransform;
    private float attackRange;
    private float attackCooldown;
    private float attackTimer = 0f;
    private bool isAttacking = false;

    public AiStateId GetId()
    {
        return AiStateId.Attack;
    }

    public void Enter(AiAgent agent)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found in Attack state");
            agent.stateMachine.ChangeState(AiStateId.Idle);
            return;
        }
        playerTransform = player.transform;

        // Get attack parameters from the attack behavior
        if (agent.attackBehavior != null)
        {
            attackRange = agent.attackBehavior.GetAttackRange();
            attackCooldown = agent.attackBehavior.GetAttackCooldown();
        }
        else
        {
            // Default values if attack behavior is missing
            attackRange = 2.0f;
            attackCooldown = 2.0f;
        }

        // Stop the agent from moving
        agent.navMeshAgent.isStopped = true;

        // Signal that we're attacking
        isAttacking = true;
        attackTimer = 0f;

        // Face the player
        if (playerTransform != null)
        {
            Vector3 direction = playerTransform.position - agent.transform.position;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                agent.transform.rotation = Quaternion.LookRotation(direction);
            }
        }

        // Set animation parameters
        if (agent.animator != null)
        {
            agent.animator.SetBool("IsAttacking", true);
            agent.animator.SetTrigger("Attack");
        }
    }

    public void Exit(AiAgent agent)
    {
        // Reset attack state
        isAttacking = false;

        // Resume agent movement
        agent.navMeshAgent.isStopped = false;

        // Reset animation parameters
        if (agent.animator != null)
        {
            agent.animator.SetBool("IsAttacking", false);
            agent.animator.ResetTrigger("Attack");
        }
    }

    public void Update(AiAgent agent)
    {
        // If no player or disabled, go back to idle
        if (playerTransform == null || !agent.enabled)
        {
            agent.stateMachine.ChangeState(AiStateId.Idle);
            return;
        }

        // Update attack timer
        attackTimer += Time.deltaTime;

        // Check distance to player
        float distanceToPlayer = Vector3.Distance(agent.transform.position, playerTransform.position);

        // If player is out of range, switch to chase state
        if (distanceToPlayer > attackRange * 1.5f)
        {
            agent.stateMachine.ChangeState(AiStateId.ChasePlayer);
            return;
        }

        // If attack cooldown is finished, we can attack again
        if (attackTimer >= attackCooldown)
        {
            // Face the player
            Vector3 direction = playerTransform.position - agent.transform.position;
            direction.y = 0;
            if (direction != Vector3.zero)
            {
                agent.transform.rotation = Quaternion.LookRotation(direction);
            }

            // Trigger attack via animation
            if (agent.animator != null)
            {
                agent.animator.SetTrigger("Attack");
            }

            // Reset timer
            attackTimer = 0f;
        }
    }
}
*/