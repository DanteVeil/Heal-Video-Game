using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiDeathState : AiState
{
    private float deathDelay = 8f;
    private float timer = 0f;

    public void Enter(AiAgent agent)
    {
        agent.navMeshAgent.isStopped = true;

        // Set the StateId and IsDead parameters in the Animator
        if (agent.animator != null)
        {
            agent.animator.SetInteger("StateId", (int)AiStateId.Death);
            agent.animator.SetBool("IsDead", true);
        }
    }

    public void Exit(AiAgent agent)
    {
        // Reset the IsDead parameter if needed
        if (agent.animator != null)
        {
            agent.animator.SetBool("IsDead", false);
        }
    }
    public AiStateId GetId()
    {
        return AiStateId.Death;
    }

    public void Update(AiAgent agent)
    {
        timer += Time.deltaTime;
        if (timer >= deathDelay)
        {
            GameObject.Destroy(agent.gameObject);
        }
    }
}