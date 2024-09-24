//==========================
// - FileName: SerializableQueue.cs
// - Created: AjieloA
// - CreateTime: 2024-09-24 14:03:43
// - Email: 1758580256@qq.com
// - Description:
//==========================
using System;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class SerializableQueue
{

}
[Serializable]
public class SerializableQueue<T> : SerializableQueue where T : UnityEngine.Object
{
    [SerializeField]
    List<T> values = new List<T>();
    public SerializableQueue()
    {
        values = new List<T>();
    }
    public virtual T Dequeue()
    {
        if (values.Count == 0)
            return null;
        T _val = values[values.Count - 1];
        values.RemoveAt(values.Count - 1);
        return _val;
    }
    public virtual void Enqueue(T obj)
    {
        values.Add(obj);
    }
    public virtual void Clear(T obj)
    {
        values.Clear();
    }
}

