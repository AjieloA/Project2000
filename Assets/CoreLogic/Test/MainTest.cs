using System;
using System.Collections.Generic;
using Tools;
using UnityEngine;

public class MainTest : MonoBehaviour
{

    void Start()
    {
        LuaEnvMgr.Instance.InitEnv();
        LuaEnvMgr.Instance.Require("Main");
    }

}
