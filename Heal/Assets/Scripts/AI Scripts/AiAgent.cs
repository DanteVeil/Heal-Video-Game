using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AiAgent : MonoBehaviour
{
    public AiStateMachine stateMachine;
    public AiStateId initialState = AiStateId.Idle; // Default to idle state
    public NavMeshAgent navMeshAgent;
    public float playerDetectionDistance = 10f; // Distance to detect the player
    public float attackRange = 2f; // Distance to start attacking
    public Animator animator; // Reference to the Animator component
    public AIAttackBehavior attackBehavior; // Reference to the attack behavior

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        attackBehavior = GetComponent<AIAttackBehavior>();

        stateMachine = new AiStateMachine(this);

        // Register all states
        stateMachine.RegisterState(new AiIdleState());
        stateMachine.RegisterState(new AiChasingState());
        stateMachine.RegisterState(new AiAttackingState());
        stateMachine.RegisterState(new AiDeathState());

        // Start with the initial state
        stateMachine.ChangeState(initialState);
    }

    void Update()
    {
        if (!enabled) return;

        stateMachine.Update();

        // Update animation parameters based on the current state
        UpdateAnimationParameters();
    }

    private void UpdateAnimationParameters()
    {
        if (animator != null)
        {
            // Set the StateId parameter based on the current state
            animator.SetInteger("StateId", (int)stateMachine.currentState);

            // Set the IsDead parameter if the AI is in the Death state
            animator.SetBool("IsDead", stateMachine.currentState == AiStateId.Death);
        }
    }
}