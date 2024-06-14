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
        CameraMgr.Instance.CameraCtrl<MainCameraCtrl>(E_CameraType.MainCamera).InitCamera();
        CameraMgr.Instance.CameraCtrl<MainCameraCtrl>(E_CameraType.MainCamera).TestCamera();
        List<int> ints = new List<int>() { 1, 2, 5, 6, 7, 9, 10 };
        LogMgr.Instance.CLog($"Random{ints.RandomListRemoveIndex(2)}");
        LogMgr.Instance.CLog($"Random{ints.RandomListRemoveIndex(2)}");
        LogMgr.Instance.CLog($"Random{ints.RandomListRemoveIndex(2)}");
        LogMgr.Instance.CLog($"Random{ints.RandomListRemoveIndex(2)}");
        LogMgr.Instance.CLog($"Random{ints.RandomListRemoveIndex(2)}");
        LogMgr.Instance.CLog($"Random{ints.RandomListRemoveIndex(2)}");
        LogMgr.Instance.CLog($"Random{ints.RandomListRemoveIndex(2)}");
        LogMgr.Instance.CLog($"Random{ints.RandomListRemoveIndex(2)}");
        LogMgr.Instance.CLog($"Random{ints.RandomListRemoveIndex(2)}");
        LogMgr.Instance.CLog($"Random{ints.RandomListRemoveIndex(2)}");
        LogMgr.Instance.CLog($"Random{ints.RandomListRemoveIndex(2)}");
        LogMgr.Instance.CLog($"Random{ints.RandomListRemoveIndex(2)}");
        LogMgr.Instance.CLog($"Random{ints.RandomListRemoveIndex(2)}");


    }

}
