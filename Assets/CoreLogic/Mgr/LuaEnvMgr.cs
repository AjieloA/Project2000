//==========================
// - FileName: LuaEnvMgr.cs
// - Created: AjieloA
// - CreateTime: 2024-06-08 18:40:52
// - Email: 1758580256@qq.com
// - Description:xlua脚本执行管理器
//==========================
using System.IO;
using UnityEngine;
using XLua;

public sealed class LuaEnvMgr : MgrBase<LuaEnvMgr>
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
            LogMgr.Instance.CError("LuaEnv Is Null");
            return;
        }
        mEnv.DoString($"require('{_fName}')");
    }
    public void DoString(string _fPath)
    {
        if (mEnv == null)
        {
            LogMgr.Instance.CError("LuaEnv Is Null");
            return;
        }
        mEnv.DoString(_fPath);
    }
    public void Tick()
    {
        if (mEnv == null)
        {
            LogMgr.Instance.CError("LuaEnv Is Null");
            return;
        }
        mEnv.Tick();
    }
    public void Dispose()
    {
        if (mEnv == null)
        {
            LogMgr.Instance.CError("LuaEnv Is Null");
            return;
        }
        mEnv.Dispose();
        mEnv = null;
    }
    private byte[] CustomLoader(ref string _fName)
    {
        string _path = $"{Application.dataPath}/CoreAsset/Xlua/{_fName}.lua";
        if (File.Exists(_path))
            return File.ReadAllBytes(_path);
        LogMgr.Instance.CError($"LuaFile Is Null,Path:{_path}");
        return null;
    }
    private byte[] CustomABLoader(ref string _fName)
    {
        TextAsset _asset = ABMgr.Instance.SyncLoadRes<TextAsset>("lua", $"{_fName}.lua");
        if (_asset != null)
            return _asset.bytes;
        LogMgr.Instance.CError($"LuaFile Is Null,Name:{_fName}");
        return null;
    }

}
