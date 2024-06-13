using UnityEngine;

public class MainTest : MonoBehaviour
{

    void Start()
    {
        LuaEnvMgr.Instance.InitEnv();
        LuaEnvMgr.Instance.Require("Main");
        CameraMgr.Instance.CameraCtrl<MainCameraCtrl>(E_CameraType.MainCamera).InitCamera();
        CameraMgr.Instance.CameraCtrl<MainCameraCtrl>(E_CameraType.MainCamera).TestCamera();

    }

}
