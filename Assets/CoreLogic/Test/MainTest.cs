using UnityEngine;

public class MainTest : MonoBehaviour
{
    BehaviorTreeBuilder behaviorTreeBuilder;
    private void Awake()
    {
        behaviorTreeBuilder = new BehaviorTreeBuilder();
    }
    void Start()
    {
        //LuaEnvMgr.Instance.InitEnv();
        //LuaEnvMgr.Instance.Require("Main");
        behaviorTreeBuilder
            .AddRepeatNode(3)
                .AddSelectorNode()
                    .AddSequenceNode()
                    .AddDebugConditionNode("1", "2")
                    .AddDebugActionNode("1")
                    .AddDebugActionNode("2")
                    .AddDebugActionNode("3")
                    .Back()
                    .AddSequenceNode()
                    .AddDebugConditionNode("1", "1")
                    .AddDebugActionNode("4")
                    .AddDebugActionNode("5")
                    .AddDebugActionNode("6")
                    .Back()
                    .AddDebugActionNode("7");
        behaviorTreeBuilder.Tick();
    }

}
