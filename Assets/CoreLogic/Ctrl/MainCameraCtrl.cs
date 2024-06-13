//==========================
// - FileName: MainCameraCtrl.cs
// - Created: AjieloA
// - CreateTime: 2024-06-11 20:16:54
// - Email: 1758580256@qq.com
// - Description:
//==========================
using Unity.VisualScripting;
using UnityEngine;

public class MainCameraCtrl : CameraCtrlBase
{
    public void TestCamera()
    {
        MCamera.transform.position = new Vector3(1, 1, 1);
        DataMgr.Instance.Get<MainCameraDataMgr>().MCameraPos = MCamera.transform.position;
    }
    public MainCameraCtrl(Camera _cam)
    {
        MCamera = _cam;
    }
    public override void InitCamera()
    {
        MCamera.transform.AddComponent<MainCameraCtrl>();
    }
    private void FixedUpdate()
    {
        transform.Translate(transform.forward * 5f * Time.fixedDeltaTime);
        DataMgr.Instance.Get<MainCameraDataMgr>().MCameraPos = transform.position;
    }
    private void LateUpdate()
    {
        LogMgr.Instance.Log($"{DataMgr.Instance.Get<MainCameraDataMgr>().MCameraPos}");
    }
}
