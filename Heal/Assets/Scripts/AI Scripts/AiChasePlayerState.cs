using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/*public class AiChasePlayerState : AiState
{
    private Transform playerTransform;
    private float timer = 0.0f;
    private float attackRange = 2.0f;

    public void Enter(AiAgent agent)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("Player not found");
            agent.enabled = false;
            return;
        }
        playerTransform = player.transform;
        timer = 0;

        // Get attack range from attack behavior if available
        if (agent.attackBehavior != null)
        {
            attackRange = agent.attackBehavior.GetAttackRange();
        }

        // Trigger chase animation if there's an animator
        if (agent.animator != null)
        {
            agent.animator.SetTrigger("StartChase");
            agent.animator.SetBool("IsChasing", true);
        }
    }

    public void Exit(AiAgent agent)
    {
        // Reset chase animation when exiting the state
        if (agent.animator != null)
        {
            agent.animator.SetBool("IsChasing", false);
        }
    }

    public AiStateId GetId()
    {
        return AiStateId.ChasePlayer;
    }

    public void Update(AiAgent agent)
    {
        if (!agent.enabled || playerTransform == null) return;

        timer -= Time.deltaTime;

        // Check if player is within attack range
        float distanceToPlayer = Vector3.Distance(agent.transform.position, playerTransform.position);
        if (distanceToPlayer <= attackRange)
        {
            // Player is within attack range, switch to attack state
            agent.stateMachine.ChangeState(AiStateId.Attack);
            return;
        }

        if (timer <= 0)
        {
            Vector3 distanceVec = playerTransform.position - agent.transform.position;
            distanceVec.y = 0;

            if (distanceVec.sqrMagnitude > agent.config.maxDistance * agent.config.maxDistance)
            {
                agent.navMeshAgent.SetDestination(playerTransform.position);

                // Update animation with distance to target
                if (agent.animator != null)
                {
                    float distanceNormalized = Mathf.Clamp01(distanceVec.magnitude / agent.config.maxDistance);
                    agent.animator.SetFloat("DistanceToTarget", distanceNormalized);
                }
            }

            timer = agent.config.maxTime;
        }
    }
}
*/