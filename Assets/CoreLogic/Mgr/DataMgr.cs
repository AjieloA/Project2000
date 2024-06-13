//==========================
// - FileName: DataMgr.cs
// - Created: AjieloA
// - CreateTime: 2024-06-12 15:07:53
// - Email: 1758580256@qq.com
// - Description:
//==========================
using System.Collections.Generic;

public sealed class DataMgr : MgrBase<DataMgr>
{
    private Dictionary<string, DataMgrBase> mDatas = new Dictionary<string, DataMgrBase>();
    public T Get<T>() where T : DataMgrBase, new()
    {
        if (!mDatas.ContainsKey(typeof(T).Name))
            mDatas.Add(typeof(T).Name, new T());
        return mDatas[typeof(T).Name] as T;
    }
    public void OnClear()
    {
        foreach (var _item in mDatas)
        {
            mDatas[_item.Key].OnClear();
        }
    }
}
