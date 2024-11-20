public class RepeatNode : DecoratorNode
{
    private int curCount;
    private int limit;
    public RepeatNode(int limit)
    {
        this.limit = limit;
    }

    protected override NodeState OnUpdate()
    {
        while (true)
        {
            curNode.Tick();
            //if (curNode.isRuning)
            //    return NodeState.Running;
            //if (curNode.isFailure)
            //    return NodeState.Failure;
            if (++curCount >= limit)
                return NodeState.Success;
        }
    }
}
public partial class BehaviorTreeBuilder
{
    public BehaviorTreeBuilder AddRepeatNode(int limit)
    {
        AddNode(new RepeatNode(limit));
        return this;
    }
}