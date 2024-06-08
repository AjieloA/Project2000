//==========================
// - FileName: SingletonAutoMono.cs
// - Created: AjieloA
// - CreateTime: 2024-06-08 18:40:52
// - Email: 1758580256@qq.com
// - Description:继承Mono的泛型单例
//==========================
using UnityEngine;
public class SingletonAutoMono<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject _mgr = new GameObject();
                GameObject _parent = GameObject.Find("Mgr");
                if (_parent == null)
                {
                    _parent = new GameObject();
                    _parent.name = "Mgr";
                    DontDestroyOnLoad(_parent);
                }
                _mgr.transform.SetParent(_parent.transform);
                _mgr.name = typeof(T).ToString();
                DontDestroyOnLoad(_mgr);
                instance = _mgr.AddComponent<T>();
            }
            return instance;
        }

    }

}
