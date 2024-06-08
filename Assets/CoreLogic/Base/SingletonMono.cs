//==========================
// - FileName: SingletonMono.cs
// - Created: AjieloA
// - CreateTime: 2024-06-08 18:40:52
// - Email: 1758580256@qq.com
// - Description:继承Mono的泛型单例
//==========================
using UnityEngine;
public class SingletonMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;

    public static T Instance
    {
        get { return instance; }
    }

    protected virtual void Awake()
    {
        instance = this as T;
    }

}
