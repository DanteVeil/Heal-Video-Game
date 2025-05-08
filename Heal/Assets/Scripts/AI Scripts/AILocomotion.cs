using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiLocomotion : MonoBehaviour
{
    private NavMeshAgent agent;
    private AiAgent aiAgent;
    private Animator animator;

    // Movement thresholds for animation transitions
    private const float MOVEMENT_THRESHOLD = 0.1f;
    private float currentSpeed = 0f;
    private bool wasMoving = false;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        aiAgent = GetComponent<AiAgent>();
        animator = GetComponent<Animator>();

        if (agent == null)
        {
            Debug.LogWarning("[AiLocomotion] Missing NavMeshAgent component!");
            enabled = false;
            return;
        }

        if (animator == null)
        {
            Debug.LogWarning("[AiLocomotion] Missing Animator component!");
        }
    }

    void Update()
    {
        // Get the current state from the state machine
        var currentStateId = aiAgent.stateMachine.currentState;

        // Handle movement based on state
        switch (currentStateId)
        {
            case AiStateId.Idle:
                // In idle state, ensure the agent is stopped
                if (!agent.isStopped)
                {
                    agent.isStopped = true;
                }
                break;

            case AiStateId.Chasing:  // Updated to match AiStateId.Chasing
                // In chase state, ensure the agent is not stopped
                if (agent.isStopped)
                {
                    agent.isStopped = false;
                }
                break;

            case AiStateId.Attacking:
                // In attack state, ensure the agent is stopped
                if (!agent.isStopped)
                {
                    agent.isStopped = true;
                }
                break;

            case AiStateId.Death:
                // In death state, ensure the agent is stopped
                if (!agent.isStopped)
                {
                    agent.isStopped = true;
                }
                break;
        }

        // Update animation parameters
        UpdateAnimation();
    }

    void UpdateAnimation()
    {
        if (animator == null) return;

        // Don't update movement animations during attack or death states
        if (aiAgent.stateMachine.currentState == AiStateId.Attacking ||
            aiAgent.stateMachine.currentState == AiStateId.Death)
        {
            return;
        }

        // Get the current speed
        currentSpeed = agent.velocity.magnitude;

        // Check if we're moving
        bool isMoving = currentSpeed > MOVEMENT_THRESHOLD;

        // Detect state changes for animation transitions
        if (isMoving != wasMoving)
        {
            wasMoving = isMoving;

            // Update the animator with movement state change
            animator.SetBool("IsMoving", isMoving);

            // Optional: If you have different movement speeds (walk/run)
            if (isMoving)
            {
                // Calculate normalized speed for blending animations
                float normalizedSpeed = Mathf.Clamp01(currentSpeed / agent.speed);
                animator.SetFloat("MovementSpeed", normalizedSpeed);
            }
        }


        // Set the current state in the animator
        animator.SetInteger("StateId", (int)aiAgent.stateMachine.currentState);
    }
}