//==========================
// - FileName: RandomTool.cs
// - Created: AjieloA
// - CreateTime: 2024-06-13 20:00:11
// - Email: 1758580256@qq.com
// - Description:
//==========================
using System;
using System.Collections.Generic;

namespace Tools
{
    public static class RandomTool
    {
        public static T RandomListBySeed<T>(List<T> _list, int _seed)
        {
            if (_list == null || _list.Count == 0)
            {
                LogMgr.Instance.Error("List Is Null Or List Count Is 0");
                return default(T);
            }
            else if (_list.Count == 1)
                return _list[0];
            Random _random = new Random(_seed);
            int _count = _random.Next(0, _list.Count);
            return _list[_count];
        }
        public static T RandomArrBySeed<T>(T[] _arr, int _seed)
        {
            if (_arr == null || _arr.Length == 0)
            {
                LogMgr.Instance.Error("Arr Is Null Or List Count Is 0");
                return default(T);
            }
            else if (_arr.Length == 1)
                return _arr[0];
            Random _random = new Random(_seed);
            int _count = _random.Next(0, _arr.Length);
            return _arr[_count];
        }
        public static T RandomList<T>(List<T> _list)
        {
            return RandomListBySeed<T>(_list, (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        }
        public static T RandomArr<T>(T[] _arr)
        {
            return RandomArrBySeed<T>(_arr, (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        }
        public static T RandomListRemoveIndexsBySeed<T>(List<T> _list, List<int> _remove, int _seed)
        {
            if (_list == null || _list.Count == 0)
            {
                LogMgr.Instance.Error("List Is Null Or List Count Is 0");
                return default(T);
            }
            else if (_list.Count == 1)
                return _list[0];
            List<T> _sort = new List<T>();
            _sort.AddRange(_list);
            if (_remove != null)
                for (int i = 0; i < _remove.Count; i++)
                    _sort.RemoveAt(_remove[i]);
            else
                LogMgr.Instance.Error("RmoveList Is Null Or List Count Is 0");
            Random _random = new Random(_seed);
            int _count = _random.Next(0, _sort.Count);
            return _sort[_count];
        }
        public static T RandomArrRemoveIndexsBySeed<T>(T[] _arr, int[] _remove, int _seed)
        {
            if (_arr == null || _arr.Length == 0)
            {
                LogMgr.Instance.Error("Arr Is Null Or List Count Is 0");
                return default(T);
            }
            else if (_arr.Length == 1)
                return _arr[0];
            T[] _sort = new T[_arr.Length - (_remove == null ? 0 : _remove.Length)];
            _sort.CopyTo(_arr, _arr.Length);
            if (_remove != null)
                for (int i = 0, k = 0; i < _arr.Length; i++)
                {
                    for (int j = 0; j < _remove.Length; j++)
                    {
                        if (i == _remove[j])
                            continue;
                    }
                    _sort[k++] = _arr[i];
                }
            else
                LogMgr.Instance.Error("RmoveList Is Null Or List Count Is 0");
            Random _random = new Random(_seed);
            int _count = _random.Next(0, _sort.Length);
            return _sort[_count];
        }
        public static T RandomListRemoveIndexs<T>(List<T> _list, List<int> _remove)
        {
            return RandomListRemoveIndexBySeed<T>(_list, _remove, (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        }
        public static T RandomArrRemoveIndexs<T>(T[] _arr, int[] _remove)
        {
            return RandomArrRemoveIndexBySeed<T>(_arr, _remove, (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        }
        public static T RandomListRemoveElementsBySeed<T>(List<T> _list, List<T> _remove, int _seed)
        {
            if (_list == null || _list.Count == 0)
            {
                LogMgr.Instance.Error("List Is Null Or List Count Is 0");
                return default(T);
            }
            else if (_list.Count == 1)
                return _list[0];
            List<T> _sort = new List<T>();
            _sort.AddRange(_list);
            if (_remove != null)
                for (int i = 0; i < _remove.Count; i++)
                    _sort.Remove(_remove[i]);
            else
                LogMgr.Instance.Error("RmoveList Is Null Or List Count Is 0");
            Random _random = new Random(_seed);
            int _count = _random.Next(0, _sort.Count);
            return _sort[_count];
        }
    }
}

