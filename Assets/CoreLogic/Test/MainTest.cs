using UnityEngine;

public class MainTest : MonoBehaviour
{

    void Start()
    {
        LuaEnvMgr.Instance.InitEnv();
        LuaEnvMgr.Instance.Require("Main");
        LogMgr.Instance.Error("testLog");
        CameraMgr.Instance.CameraCtrl<MainCameraCtrl>(E_CameraType.MainCamera).MCamera = Camera.main;
        CameraMgr.Instance.CameraCtrl<MainCameraCtrl>(E_CameraType.MainCamera).TestCamera();
    }

}
