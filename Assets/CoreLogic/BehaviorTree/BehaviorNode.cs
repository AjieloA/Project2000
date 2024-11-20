using UnityEngine;

public abstract class BehaviorNode : ScriptableObject
{
    public NodeState curState = NodeState.Failure;
    public bool isRuning => curState == NodeState.Running;
    public bool isSuccess => curState == NodeState.Success;
    public bool isFailure => curState == NodeState.Failure;
    protected virtual void OnEnter() { }
    protected virtual void OnExit() { }
    protected abstract NodeState OnUpdate();
    public virtual NodeState Tick()
    {
        if (!isRuning)
            OnEnter();
        curState = OnUpdate();
        if (!isRuning)
            OnExit();
        return curState;
    }
    public virtual void AddNode(BehaviorNode node) { }
    public virtual void RemoveNode(BehaviorNode node) { }

}
public enum NodeState
{
    Running,
    Success,
    Failure
}
