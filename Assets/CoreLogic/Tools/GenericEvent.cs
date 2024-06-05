using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public sealed class GenericEvent<T> where T : Object
{
    private readonly Dictionary<string, UnityAction<T>> mEvents = new Dictionary<string, UnityAction<T>>();
    public GenericEvent()
    {
        mEvents.Clear();
    }
    public void Append(MonoBehaviour _mono, UnityAction<T> _callBack)
    {
        if (_callBack == null)
            return;
        if (mEvents.ContainsKey(_mono.name) && mEvents[_mono.name] == _callBack)
        {
            Debug.LogError($"{_mono.name}重复添加事件");
        }
        else
        {
            mEvents.Add(_mono.name, _callBack);
        }
    }
    public void Trigger(T _t)
    {
        foreach (var _item in mEvents)
        {
            _item.Value?.Invoke(_t);
        }
    }
    public void Remove(MonoBehaviour _mono)
    {
        if (mEvents.ContainsKey(_mono.name))
        {
            mEvents.Remove(_mono.name);
        }
    }
    public void Clear()
    {
        mEvents.Clear();
    }
}
