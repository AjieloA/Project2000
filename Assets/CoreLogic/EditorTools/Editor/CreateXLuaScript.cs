//==========================
// - FileName: CreateXLuaScript.cs
// - Created: AjieloA
// - CreateTime: 2024-07-07 14:35:18
// - Email: 1758580256@qq.com
// - Description:
//==========================

using System.IO;
using UnityEditor;
using UnityEngine;

public class CreateXLuaScript
{
    private static readonly string mRootPath = @"Assets/CoreAsset/XLua";
    [MenuItem("Assets/UIKits/CreateUIPanelScript")]
    public static void CreateUIPanel()
    {
        GameObject _obj = Selection.activeGameObject;
        string _rootPath = $"{mRootPath}/{_obj.name}";
        string _panelPath = $"{_rootPath}/{_obj.name}.lua";
        string _panelBasePath = $"{_rootPath}/{_obj.name}Base.lua";

        if (!Directory.Exists(_rootPath))
            Directory.CreateDirectory(_rootPath);
        if (!File.Exists(_panelPath))
        {
            string _luaScripts = $"{_obj.name} = {{}}\r\n{_obj.name}.__index = {_obj.name}\r\n{_obj.name} = setmetatable({_obj.name}Base, {_obj.name})\r\nfunction {_obj.name}:RegisterEvent() end\r\nfunction {_obj.name}:UnRegisterEvent() end\r\n";
            File.WriteAllText(_panelPath, _luaScripts);
        }
        if (File.Exists(_panelBasePath))
            File.Delete(_panelBasePath);
        string _findPath = "";
        GetEelement(_obj.transform, "", ref _findPath);
        string _luaBase = $"{_obj.name}Base = {{}}\r\n{_obj.name}Base.__index = {_obj.name}Base\r\n{_obj.name}Base = setmetatable(UIViewBase, {_obj.name}Base)\r\nfunction {_obj.name}Base:Find(...)\r\n{_findPath}\r\nend";
        File.WriteAllText(_panelBasePath, _luaBase);


    }
    static void GetEelement(Transform _parent, string _path, ref string _fullStr)
    {
        string _str = _path;
        int _index = 0;
        string _head = "";
        for (int i = 0; i < _parent.childCount; i++)
        {
            _index = 0;
            _head = "";
            Transform _child = _parent.GetChild(i);
            if (_str == "")
                _str += _child.name;
            else
                _str += $"/{_child.name}";
            _index = _child.name.IndexOf("_");
            if (_index > 0)
            {
                _head = _child.name.Substring(0, _index);
                if (_head != "")
                {
                    _head = GetElementType(_head);
                    if (_head != "")
                        _fullStr += $"self.m{_child.name.Substring(0, 1).ToUpper() + _child.name.Substring(1)}=self.mView.transform:Find(\"{_str}\"):GetComponent(typeof({_head}))\r\n";
                }
            }
            if (_child.childCount == 0)
                continue;
            GetEelement(_child, _str, ref _fullStr);
        }
    }
    static string GetElementType(string _type)
    {
        return _type switch
        {
            "tra" => "RectTransform",
            "btn" => "Button",
            "img" => "Image",
            _ => ""
        };
    }
}
