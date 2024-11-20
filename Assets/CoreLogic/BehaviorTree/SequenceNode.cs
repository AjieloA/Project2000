/// <summary>
/// ˳��ڵ� ����˳��ִ�ж���ӽڵ� �����ӽڵ㶼�ɹ�ִ�л�һ���ӽڵ�ִ��ʧ��ʱ��ֹ
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
