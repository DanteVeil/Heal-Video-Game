using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiChasingState : AiState
{
    private Transform playerTransform;

    public void Enter(AiAgent agent)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        agent.navMeshAgent.isStopped = false;

        // Set the StateId parameter in the Animator
        if (agent.animator != null)
        {
            agent.animator.SetInteger("StateId", (int)AiStateId.Chasing);
        }
    }

    public void Exit(AiAgent agent)
    {
        // Reset the StateId parameter if needed
        if (agent.animator != null)
        {
            agent.animator.SetInteger("StateId", (int)AiStateId.Idle);
        }
    }

    public AiStateId GetId()
    {
        return AiStateId.Chasing;
    }

    public void Update(AiAgent agent)
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(agent.transform.position, playerTransform.position);
        if (distanceToPlayer <= agent.attackRange)
        {
            agent.stateMachine.ChangeState(AiStateId.Attacking);
        }
        else
        {
            agent.navMeshAgent.SetDestination(playerTransform.position);
        }
    }
}