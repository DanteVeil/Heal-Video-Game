using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AiStateId
{
    Idle = 0,
    Chasing = 1,
    Attacking = 2,
    Death = 3
}

public interface AiState
{
    AiStateId GetId();
    void Enter(AiAgent agent);
    void Exit(AiAgent agent);
    void Update(AiAgent agent);
}