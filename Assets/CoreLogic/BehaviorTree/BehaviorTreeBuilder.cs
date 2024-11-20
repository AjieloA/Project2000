using System.Collections.Generic;

public partial class BehaviorTreeBuilder
{
    public BehaviorNode rootNode;
    public readonly Stack<BehaviorNode> nodeStack;
    public BehaviorTreeBuilder()
    {
        rootNode = null;
        nodeStack = new Stack<BehaviorNode>();
    }
    public void AddNode(BehaviorNode node)
    {
        if (rootNode == null)
            rootNode = node;
        else
            nodeStack.Peek().AddNode(node);
        if (node is DecoratorNode || node is CompositeNode)
            nodeStack.Push(node);
    }
    public BehaviorTreeBuilder Back()
    {
        nodeStack.Pop();
        return this;
    }
    public BehaviorTreeBuilder Clear()
    {
        nodeStack.Clear();
        return this;
    }
    public void Tick()
    {
        rootNode.Tick();
    }

}
