using NUnit.Framework;
using UnityEngine;

public class AiStateMachineTests
{
    private AiAgent dummyAgent;
    private AiStateMachine stateMachine;

    private class DummyState : AiState
    {
        public bool entered = false, exited = false;

        public AiStateId GetId() => AiStateId.Idle;
        public void Enter(AiAgent agent) { entered = true; }
        public void Exit(AiAgent agent) { exited = true; }
        public void Update(AiAgent agent) { }
    }



    [SetUp]
    public void Setup()
    {
        dummyAgent = new GameObject().AddComponent<AiAgent>();
        stateMachine = new AiStateMachine(dummyAgent);
    }

    [Test]
    public void RegistersAndChangesState()
    {
        var state = new DummyState();
        stateMachine.RegisterState(state);
        stateMachine.ChangeState(AiStateId.Idle);
        Assert.IsTrue(state.entered);
    }
}
