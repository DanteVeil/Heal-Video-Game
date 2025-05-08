using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiIdleState : AiState
{
    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.isStopped = true;

        // Set the StateId parameter in the Animator
        if (agent.animator != null)
        {
            agent.animator.SetInteger("StateId", (int)AiStateId.Idle);
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
        return AiStateId.Idle;
    }

    public void Update(AiAgent agent)
    {
        var player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float distanceToPlayer = Vector3.Distance(agent.transform.position, player.transform.position);
            if (distanceToPlayer <= agent.playerDetectionDistance)
            {
                agent.stateMachine.ChangeState(AiStateId.Chasing);
            }
        }
    }
}