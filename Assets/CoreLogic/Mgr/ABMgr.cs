using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ABMgr : SingletonAutoMono<ABMgr>
{
    private Dictionary<string, AssetBundle> mABResDic = new Dictionary<string, AssetBundle>();
    private AssetBundle mAB = null;
    private AssetBundleManifest mManifest = null;
    private string mPathUrl
    {
        get
        {
            return Application.streamingAssetsPath + '/';
        }
    }
    private string mMainABName
    {
        get
        {
#if UNITY_IOS
                        return "IOS";
#elif UNITY_ANDROID
                        return "Android";
#else
            return "PC";
#endif
        }
    }
    private void SyncLoadAB(string _abName)
    {
        /*加载AB包
                 * 获取依赖包信息
                 * 加载主包
                 * 加载主包中的依赖文件
                 * 加载依赖包
                 * 加载目标包
                 */
        if (mAB == null)
        {
            mAB = AssetBundle.LoadFromFile(mPathUrl + mMainABName);
            mManifest = mAB.LoadAsset<AssetBundleManifest>("AssetBundleManifest");
        }
        //获取依赖包信息
        string[] _strs = mManifest.GetAllDependencies(_abName);
        for (int i = 0; i < _strs.Length; i++)
        {
            //判断包是否被加载过
            if (mABResDic.ContainsKey(_strs[i]))
                continue;
            AssetBundle _ab = AssetBundle.LoadFromFile(mPathUrl + _strs[i]);
            mABResDic.Add(_strs[i], _ab);
        }
        //加载资源来源包
        if (!mABResDic.ContainsKey(_abName))
        {
            AssetBundle _abRes = AssetBundle.LoadFromFile(mPathUrl + _abName);
            mABResDic.Add(_abName, _abRes);
        }
    }
    #region 同步加载
    //同步加载 不指定类型加载
    public Object SyncLoadRes(string _abName, string _resName)
    {
        SyncLoadAB(_abName);
        //加载资源
        return mABResDic[_abName].LoadAsset(_resName);
    }

    //同步加载 泛型加载
    public T SyncLoadRes<T>(string _abName, string _resName) where T : Object
    {
        SyncLoadAB(_abName);
        //加载资源
        return mABResDic[_abName].LoadAsset<T>(_resName);
    }

    //同步加载 按照资源类型加载
    public Object SyncLoadRes(string _abName, string _resName, System.Type _type)
    {
        SyncLoadAB(_abName);
        //加载资源
        return mABResDic[_abName].LoadAsset(_resName, _type);
    }
    #endregion
    #region 异步加载内的资源
    //异步加载
    public void AsyncLoadRes(string _abName, string _resName, UnityAction<Object> _callBck)
    {
        StartCoroutine(AsyncLoadABResIE(_abName, _resName, _callBck));
    }
    public void AsyncLoadRes(string _abName, string _resName, System.Type _type, UnityAction<Object> _callBck)
    {
        StartCoroutine(AsyncLoadABResIE(_abName, _resName, _type, _callBck));
    }
    public void AsyncLoadRes<T>(string _abName, string _resName, UnityAction<T> _callBck) where T : Object
    {
        StartCoroutine(AsyncLoadABResIE<T>(_abName, _resName, _callBck));
    }
    private IEnumerator AsyncLoadABResIE(string _abName, string _resName, UnityAction<Object> _callBck)
    {
        SyncLoadAB(_abName);
        AssetBundleRequest _res = mABResDic[_abName].LoadAssetAsync(_resName);
        yield return _res;
        _callBck?.Invoke(_res.asset);
    }
    private IEnumerator AsyncLoadABResIE(string _abName, string _resName, System.Type _type, UnityAction<Object> _callBck)
    {
        SyncLoadAB(_abName);
        AssetBundleRequest _res = mABResDic[_abName].LoadAssetAsync(_resName, _type);
        yield return _res;
        _callBck?.Invoke(_res.asset);
    }
    private IEnumerator AsyncLoadABResIE<T>(string _abName, string _resName, UnityAction<T> _callBck) where T : Object
    {
        SyncLoadAB(_abName);
        AssetBundleRequest _res = mABResDic[_abName].LoadAssetAsync<T>(_resName);
        yield return _res;
        _callBck?.Invoke(_res.asset as T);
    }
    #endregion
    #region 异步加载AB包和内部的资源
    private void AsyncLoadABLaunch(string _abName, string _resName, UnityAction _callBck)
    {
        StartCoroutine(AsyncLoadABLanuchIE(_abName, _resName, _callBck));
    }
    private IEnumerator AsyncLoadABLanuchIE(string _abName, string _resName, UnityAction _callBck)
    {
        if (mAB == null)
        {
            AssetBundleCreateRequest _tempMain = AssetBundle.LoadFromFileAsync(mPathUrl + mMainABName);
            yield return _tempMain;
            mAB = _tempMain.assetBundle;
            AssetBundleRequest _tempManifest = mAB.LoadAssetAsync<AssetBundleManifest>("AssetBundleManifest");
            yield return _tempManifest;
            mManifest = _tempManifest.asset as AssetBundleManifest;
        }
        string[] _pageName = mManifest.GetAllDependencies(_abName);
        for (int i = 0; i < _pageName.Length; i++)
        {
            if (mABResDic.ContainsKey(_pageName[i]))
                continue;
            AssetBundleCreateRequest _tempAB = AssetBundle.LoadFromFileAsync(mPathUrl + _pageName[i]);
            yield return _tempAB;
            mABResDic.Add(_pageName[i], _tempAB.assetBundle);
        }
        if (!mABResDic.ContainsKey(_abName))
        {
            AssetBundleCreateRequest _tempAB = AssetBundle.LoadFromFileAsync(mPathUrl + _abName);
            yield return _tempAB;
            mABResDic.Add(_abName, _tempAB.assetBundle);
        }
        _callBck?.Invoke();
        yield return null;
    }

    public void AsyncLoadAB(string _abName, string _resName, UnityAction<Object> _callBck)
    {
        AsyncLoadABLaunch(_abName, _resName, () =>
        {
            StartCoroutine(AsyncLoadABResIE(_abName, _resName, _callBck));
        });
    }
    public void AsyncLoadAB<T>(string _abName, string _resName, UnityAction<T> _callBck) where T : Object
    {
        AsyncLoadABLaunch(_abName, _resName, () =>
        {
            StartCoroutine(AsyncLoadABResIE<T>(_abName, _resName, _callBck));
        });
    }
    public void AsyncLoadAB(string _abName, string _resName,System.Type _type, UnityAction<Object> _callBck)
    {
        AsyncLoadABLaunch(_abName, _resName, () =>
        {
            StartCoroutine(AsyncLoadABResIE(_abName, _resName, _type, _callBck));
        });
    }
    #endregion
    //单个资源包卸载
    public bool UnLoadSingle(string _abName, bool _isUnLoadAsset = false)
    {
        if (!mABResDic.ContainsKey(_abName))
            return false;
        mABResDic[_abName].Unload(_isUnLoadAsset);
        mABResDic.Remove(_abName);
        return true;
    }
    //所有资源包卸载
    public bool UnLoadAll(string _abName, bool _isUnLoadAsset = false)
    {
        AssetBundle.UnloadAllAssetBundles(_isUnLoadAsset);
        mABResDic.Clear();
        mAB = null;
        mManifest = null;
        return true;
    }
}
