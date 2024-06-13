//==========================
// - FileName: CameraMgr.cs
// - Created: AjieloA
// - CreateTime: 2024-06-11 20:13:54
// - Email: 1758580256@qq.com
// - Description:
//==========================
using System.Collections.Generic;
using UnityEngine;

public class CameraMgr : MgrBase<CameraMgr>
{
    private Dictionary<E_CameraType, CameraCtrlBase> mCameras = new Dictionary<E_CameraType, CameraCtrlBase>()
    {
        {E_CameraType.MainCamera,new MainCameraCtrl(Camera.main) }
    };
    public T CameraCtrl<T>(E_CameraType _key) where T : CameraCtrlBase
    {
        CameraCtrlBase ctrl = null;
        if (!mCameras.TryGetValue(_key, out ctrl))
        {
            LogMgr.Instance.Error($"CameraCtrl is Null,Key:{_key}");
        }
        return ctrl as T;
    }
}
public enum E_CameraType
{
    None = 0,
    MainCamera = 1,//Ö÷Ïà»ú

}
