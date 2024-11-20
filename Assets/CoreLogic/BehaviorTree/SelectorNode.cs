/// <summary>
/// 选择节点 一个节点执行成功后返回
/// </summary>
public class SelectorNode : CompositeNode
{
    protected override NodeState OnUpdate()
    {
        while (true)
        {
            curNode.Value.Tick();
            if (curNode.Value.isSuccess)
                return curState;
            curNode = curNode.Next;
            if (curNode == null)
                return curState = NodeState.Failure;
        }
    }
}
public partial class BehaviorTreeBuilder
{
    public BehaviorTreeBuilder AddSelectorNode()
    {
        AddNode(new SelectorNode());
        return this;
    }
}