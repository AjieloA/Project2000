using System.IO;
using UnityEngine;
using XLua;

public sealed class LuaEnvMgr : BaseMgr<LuaEnvMgr>
{
    private LuaEnv mEnv = null;
    public LuaTable LuaTable
    {
        get => mEnv.Global;
    }
    public void InitEnv()
    {
        if (mEnv != null)
            return;
        mEnv = new LuaEnv();
        mEnv.AddLoader(CustomLoader);
        //mEnv.AddLoader(CustomABLoader);
    }
    public void Require(string _fName)
    {
        if (mEnv == null)
        {
            Debug.Log("LuaEnv Is Null");
            return;
        }
        mEnv.DoString($"require('{_fName}')");
    }
    public void DoString(string _fPath)
    {
        if (mEnv == null)
        {
            Debug.Log("LuaEnv Is Null");
            return;
        }
        mEnv.DoString(_fPath);
    }
    public void Tick()
    {
        if (mEnv == null)
        {
            Debug.Log("LuaEnv Is Null");
            return;
        }
        mEnv.Tick();
    }
    public void Dispose()
    {
        if (mEnv == null)
        {
            Debug.Log("LuaEnv Is Null");
            return;
        }
        mEnv.Dispose();
        mEnv = null;
    }
    private byte[] CustomLoader(ref string _fName)
    {
        string _path = $"{Application.dataPath}/CoreLogic/Xlua/{_fName}.lua";
        if (File.Exists(_path))
            return File.ReadAllBytes(_path);
        Debug.Log($"LuaFile Is Null,Path:{_path}");
        return null;
    }
    private byte[] CustomABLoader(ref string _fName)
    {
        TextAsset _asset = ABMgr.Instance.SyncLoadRes<TextAsset>("lua", $"{_fName}.lua");
        if (_asset != null)
            return _asset.bytes;
        Debug.Log($"LuaFile Is Null,Name:{_fName}");
        return null;
    }

}
