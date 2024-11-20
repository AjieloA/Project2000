using System.Collections.Generic;

/// <summary>
/// ��Ͻڵ�
/// </summary>
public abstract class CompositeNode : BehaviorNode
{
    protected LinkedList<BehaviorNode> nodes;
    protected LinkedListNode<BehaviorNode> curNode;
    public CompositeNode()
    {
        nodes = new LinkedList<BehaviorNode>();
    }
    protected override void OnEnter()
    {
        curNode = nodes.First;
    }
    public override void AddNode(BehaviorNode node)
    {
        nodes.AddLast(node);
    }
    public override void RemoveNode(BehaviorNode node)
    {
        nodes.Remove(node);
    }
}
