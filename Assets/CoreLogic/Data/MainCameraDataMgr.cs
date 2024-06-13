//==========================
// - FileName: MainCameraDataMgr.cs
// - Created: AjieloA
// - CreateTime: 2024-06-12 16:32:50
// - Email: 1758580256@qq.com
// - Description:
//==========================
using UnityEngine;

public sealed class MainCameraDataMgr : DataMgrBase
{
    private Vector3 mCameraPos = Vector3.zero;

    public Vector3 MCameraPos { get => mCameraPos; set => mCameraPos = value; }

    public override void OnClear()
    {
    }
}
