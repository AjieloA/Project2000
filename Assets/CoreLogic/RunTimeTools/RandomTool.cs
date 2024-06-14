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
        static int mSeed = 1;
        public static T RandomListBySeed<T>(this List<T> _list, int _seed)
        {
            if (_list == null || _list.Count == 0)
            {
                LogMgr.Instance.CError("List Is Null Or List Count Is 0");
                return default(T);
            }
            else if (_list.Count == 1)
                return _list[0];
            Random _random = new Random(_seed);
            int _count = _random.Next(0, _list.Count);
            return _list[_count];
        }
        public static T RandomArrBySeed<T>(this T[] _arr, int _seed)
        {
            if (_arr == null || _arr.Length == 0)
            {
                LogMgr.Instance.CError("Arr Is Null Or List Count Is 0");
                return default(T);
            }
            else if (_arr.Length == 1)
                return _arr[0];
            Random _random = new Random(_seed);
            int _count = _random.Next(0, _arr.Length);
            return _arr[_count];
        }
        public static T RandomList<T>(this List<T> _list)
        {
            return RandomListBySeed<T>(_list, (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() + mSeed++);
        }
        public static T RandomArr<T>(this T[] _arr)
        {
            return RandomArrBySeed<T>(_arr, (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() + mSeed++);
        }
        public static T RandomListRemoveIndexBySeed<T>(this List<T> _list, int _remove, int _seed)
        {
            if (_list == null || _list.Count == 0)
            {
                LogMgr.Instance.CError("List Is Null Or List Count Is 0");
                return default(T);
            }
            else if (_list.Count == 1)
                return _list[0];
            List<T> _sort = new List<T>();
            _sort.AddRange(_list);
            _sort.RemoveAt(_remove);
            Random _random = new Random(_seed);
            int _count = _random.Next(0, _sort.Count);
            return _sort[_count];
        }
        public static T RandomArrRemoveIndexBySeed<T>(this T[] _arr, int _remove, int _seed)
        {
            if (_arr == null || _arr.Length == 0)
            {
                LogMgr.Instance.CError("Arr Is Null Or List Count Is 0");
                return default(T);
            }
            else if (_arr.Length == 1)
                return _arr[0];
            int _lens = 1;
            T[] _sort = new T[_arr.Length];
            for (int i = 0, k = 0; i < _arr.Length; i++)
            {
                if (i == _remove)
                    continue;
                _sort[k++] = _arr[i];
                _lens++;
            }
            Random _random = new Random(_seed);
            int _count = _random.Next(0, _lens);
            return _sort[_count];
        }
        public static T RandomListRemoveIndex<T>(this List<T> _list, int _remove)
        {
            return RandomListRemoveIndexBySeed(_list, _remove, (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() + mSeed++);
        }
        public static T RandomArrRemoveIndex<T>(this T[] _arr, int _remove)
        {
            return RandomArrRemoveIndexBySeed(_arr, _remove, (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() + mSeed++);

        }
        public static T RandomListRemoveIndexsBySeed<T>(this List<T> _list, List<int> _remove, int _seed)
        {
            if (_list == null || _list.Count == 0)
            {
                LogMgr.Instance.CError("List Is Null Or List Count Is 0");
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
                LogMgr.Instance.CError("RmoveList Is Null Or List Count Is 0");
            Random _random = new Random(_seed);
            int _count = _random.Next(0, _sort.Count);
            return _sort[_count];
        }
        public static T RandomArrRemoveIndexsBySeed<T>(this T[] _arr, int[] _remove, int _seed)
        {
            if (_arr == null || _arr.Length == 0)
            {
                LogMgr.Instance.CError("Arr Is Null Or List Count Is 0");
                return default(T);
            }
            else if (_arr.Length == 1)
                return _arr[0];
            T[] _sort = new T[_arr.Length];
            int _lens = 1;
            if (_remove != null)
                for (int i = 0, k = 0; i < _arr.Length; i++)
                {
                    for (int j = 0; j < _remove.Length; j++)
                    {
                        if (i == _remove[j])
                            continue;
                    }
                    _sort[k++] = _arr[i];
                    _lens++;
                }
            else
                LogMgr.Instance.CError("RmoveList Is Null Or List Count Is 0");
            Random _random = new Random(_seed);
            int _count = _random.Next(0, _lens);
            return _sort[_count];
        }
        public static T RandomListRemoveIndexs<T>(this List<T> _list, List<int> _remove)
        {
            return RandomListRemoveIndexsBySeed<T>(_list, _remove, (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() + mSeed++);
        }
        public static T RandomArrRemoveIndexs<T>(this T[] _arr, int[] _remove)
        {
            return RandomArrRemoveIndexsBySeed<T>(_arr, _remove, (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() + mSeed++);
        }
        public static T RandomListRemoveElementBySeed<T>(this List<T> _list, T _remove, int _seed)
        {
            if (_list == null || _list.Count == 0)
            {
                LogMgr.Instance.CError("List Is Null Or List Count Is 0");
                return default(T);
            }
            else if (_list.Count == 1)
                return _list[0];
            List<T> _sort = new List<T>();
            _sort.AddRange(_list);
            if (_remove != null)
                _sort.Remove(_remove);
            else
                LogMgr.Instance.CError("RmoveList Is Null Or List Count Is 0");
            Random _random = new Random(_seed);
            int _count = _random.Next(0, _sort.Count);
            return _sort[_count];
        }
        public static T RandomArrRemoveElementBySeed<T>(this T[] _arr, T _remove, int _seed, Func<T, T, bool> _sortFunc)
        {
            if (_arr == null || _arr.Length == 0)
            {
                LogMgr.Instance.CError("Arr Is Null Or List Count Is 0");
                return default(T);
            }
            else if (_arr.Length == 1)
                return _arr[0];

            T[] _sort = new T[_arr.Length];
            int _lens = 0;
            if (_remove != null)
                for (int i = 0, k = 0; i < _arr.Length; i++)
                {
                    if (_sortFunc(_arr[i], _remove))
                        continue;
                    _sort[k++] = _arr[i];
                    _lens++;
                }
            else
                LogMgr.Instance.CError("RmoveList Is Null Or List Count Is 0");
            Random _random = new Random(_seed);
            int _count = _random.Next(0, _lens);
            return _sort[_count];
        }
        public static T RandomListRemoveElement<T>(this List<T> _list, T _remove, int _seed)
        {
            return RandomListRemoveElementBySeed(_list, _remove, (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() + mSeed++);
        }
        public static T RandomArrRemoveElement<T>(this T[] _arr, T _remove, int _seed, Func<T, T, bool> _sortFunc)
        {
            return RandomArrRemoveElementBySeed(_arr, _remove, (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() + mSeed++, _sortFunc);

        }
        public static T RandomListRemoveElementsBySeed<T>(this List<T> _list, List<T> _remove, int _seed)
        {
            if (_list == null || _list.Count == 0)
            {
                LogMgr.Instance.CError("List Is Null Or List Count Is 0");
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
                LogMgr.Instance.CError("RmoveList Is Null Or List Count Is 0");
            Random _random = new Random(_seed);
            int _count = _random.Next(0, _sort.Count);
            return _sort[_count];
        }
        public static T RandomArrRemoveElementsBySeed<T>(this T[] _arr, T[] _remove, int _seed, Func<T, T, bool> _sortFunc)
        {
            if (_arr == null || _arr.Length == 0)
            {
                LogMgr.Instance.CError("Arr Is Null Or List Count Is 0");
                return default(T);
            }
            else if (_arr.Length == 1)
                return _arr[0];

            T[] _sort = new T[_arr.Length];
            int _lens = 0;
            if (_remove != null)
                for (int i = 0, k = 0; i < _arr.Length; i++)
                {
                    for (int j = 0; j < _remove.Length; j++)
                    {
                        if (_sortFunc(_arr[i], _remove[j]))
                            continue;
                    }
                    _sort[k++] = _arr[i];
                    _lens++;
                }
            else
                LogMgr.Instance.CError("RmoveList Is Null Or List Count Is 0");
            Random _random = new Random(_seed);
            int _count = _random.Next(0, _lens);
            return _sort[_count];
        }
        public static T RandomListRemoveElements<T>(this List<T> _list, List<T> _remove)
        {
            return RandomListRemoveElementsBySeed(_list, _remove, (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() + mSeed++);
        }
        public static T RandomArrRemoveElements<T>(this T[] _arr, T[] _remove, Func<T, T, bool> _sortFunc)
        {
            return RandomArrRemoveElementsBySeed(_arr, _remove, (int)DateTimeOffset.UtcNow.ToUnixTimeSeconds() + mSeed++, _sortFunc);
        }
    }
}

