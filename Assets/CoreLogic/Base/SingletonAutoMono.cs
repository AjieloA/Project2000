using UnityEngine;

//C#中 泛型知识点
//设计模式 单例模式的知识点
//继承这种自动创建的 单例模式基类 不需要我们手动去拖 或者 api去加了
//想用他 直接 GetInstance就行了
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
