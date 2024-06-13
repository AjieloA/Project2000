//==========================
// - FileName: CameraCtrlBase.cs
// - Created: AjieloA
// - CreateTime: 2024-06-11 20:16:31
// - Email: 1758580256@qq.com
// - Description:
//==========================
using UnityEngine;

public class CameraCtrlBase : MonoBehaviour
{
    private Camera mCamera;
    public Camera MCamera { get => mCamera; set => mCamera = value; }
    public virtual void InitCamera()
    {

    }
}
