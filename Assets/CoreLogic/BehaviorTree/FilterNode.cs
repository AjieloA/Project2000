public class FilterNode : SequenceNode
{
    public override void AddNode(BehaviorNode node)
    {
        if (node is ConditionNode)
            nodes.AddFirst(node);
        else
            nodes.AddLast(node);
    }
}
public partial class BehaviorTreeBuilder
{
    public BehaviorTreeBuilder AddFilterNode()
    {
        AddNode(new FilterNode());
        return this;
    }
}