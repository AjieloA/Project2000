public sealed class DebugActionNode : ActionNode
{
    private string str;
    public DebugActionNode(string str)
    {
        this.str = str;
    }
    protected override NodeState OnUpdate()
    {
        LogMgr.Instance.CLog(this.str);
        return NodeState.Success;
    }
}
public partial class BehaviorTreeBuilder
{
    public BehaviorTreeBuilder AddDebugActionNode(string str)
    {
        AddNode(new DebugActionNode(str));
        return this;
    }
}