//==========================
// - FileName: SimpleEvent.cs
// - Created: AjieloA
// - CreateTime: 2024-06-08 18:40:52
// - Email: 1758580256@qq.com
// - Description:简单事件
//==========================
using System.Collections.Generic;
using UnityEngine;
namespace Tools
{
    public sealed class SimpleEvent
    {
        private readonly Dictionary<string, mAction> mEvents = new Dictionary<string, mAction>();
        public SimpleEvent()
        {
            mEvents.Clear();
        }
        public void Append(MonoBehaviour _mono, mAction _callBack)
        {
            if (_callBack == null)
                return;
            if (mEvents.ContainsKey(_mono.name) && mEvents[_mono.name] == _callBack)
            {
                LogMgr.Instance.CError($"{_mono.name}重复添加事件");
            }
            else
            {
                mEvents.Add(_mono.name, _callBack);
            }
        }
        public void Trigger()
        {
            foreach (var _item in mEvents)
            {
                _item.Value?.Invoke();
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
}

