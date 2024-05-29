//1.C#中 泛型的知识
//2.设计模式中 单例模式的知识
public class BaseMgr<T> where T : new()
{
    private static T instance;

    public static T Instance
    {
        get
        {
            if (instance == null)
                instance = new T();
            return instance;
        }
    }
}

