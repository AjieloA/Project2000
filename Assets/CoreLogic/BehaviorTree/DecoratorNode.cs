/// <summary>
/// в╟йн╫з╣Ц
/// </summary>
public abstract class DecoratorNode : BehaviorNode
{
    protected BehaviorNode curNode;
    public override void AddNode(BehaviorNode node)
    {
        curNode = node;
    }
}
