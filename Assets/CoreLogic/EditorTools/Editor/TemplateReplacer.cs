//==========================
// - FileName: TemplateReplacer.cs
// - Created: AjieloA
// - CreateTime: 2024-06-08 18:40:52
// - Email: 1758580256@qq.com
// - Description:脚本创建模板替换
//==========================
using System.IO;
using System;
using UnityEditor;

[Obsolete]
public class TemplateReplacer : AssetModificationProcessor
{
    // 创建脚本时调用
    public static void OnWillCreateAsset(string path)
    {
        // 只处理 C# 脚本文件
        path = path.Replace(".meta", "");
        if (path.EndsWith(".cs"))
        {
            // 延迟调用以确保文件创建完成
            EditorApplication.delayCall += () => ProcessScript(path);
        }
    }

    private static void ProcessScript(string path)
    {
        // 读取脚本内容
        string content = File.ReadAllText(path);

        // 获取当前日期
        string currentDate = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // 替换占位符
        content = content.Replace("#AUTHORNAME#", "AjieloA"); // 替换为实际作者名字
        content = content.Replace("#CREATIONDATE#", currentDate);
        content = content.Replace("#AUTHOREMAIL#", "1758580256@qq.com");

        // 写回文件
        File.WriteAllText(path, content);

        // 刷新资产数据库
        AssetDatabase.Refresh();
    }
}
