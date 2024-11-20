public sealed class DebugConditionNode : ConditionNode
{
    string target;
    string result;
    public DebugConditionNode(string target, string result)
    {
        this.target = target;
        this.result = result;
    }
    protected override NodeState OnUpdate()
    {

        if (target == result)
            return curState = NodeState.Success;
        return curState = NodeState.Failure;
    }
}
public partial class BehaviorTreeBuilder
{
    public BehaviorTreeBuilder AddDebugConditionNode(string target, string result)
    {
        AddNode(new DebugConditionNode(target, result));
        return this;
    }
}