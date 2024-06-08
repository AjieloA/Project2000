using UnityEngine;

public class MainTest : MonoBehaviour
{

    void Start()
    {
        LuaEnvMgr.Instance.InitEnv();
        LuaEnvMgr.Instance.Require("Main");
    }
    //void Update()
    //{

    //}
}
