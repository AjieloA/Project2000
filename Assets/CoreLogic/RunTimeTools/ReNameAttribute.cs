//==========================
// - FileName: ReNameAttribute.cs
// - Created: AjieloA
// - CreateTime: 2024-09-23 11:46:31
// - Email: 1758580256@qq.com
// - Description:
//==========================
using UnityEngine;

public class ReNameAttribute : PropertyAttribute
{
    public string mName = "";
    public ReNameAttribute(string _name)
    {
        mName = _name;
    }
}
