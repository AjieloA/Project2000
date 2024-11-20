/// <summary>
/// 顺序节点 按照顺序执行多个子节点 所有子节点都成功执行或一个子节点执行失败时终止
/// </summary>
public class SequenceNode : CompositeNode
{
    protected override NodeState OnUpdate()
    {
        while (true)
        {
            curNode.Value.Tick();
            if (!curNode.Value.isSuccess)
                return curState;
            curNode = curNode.Next;
            if (curNode == null)
                return NodeState.Success;
        }
    }
}
public partial class BehaviorTreeBuilder
{
    public BehaviorTreeBuilder AddSequenceNode()
    {
        AddNode(new SequenceNode());
        return this;
    }
}
