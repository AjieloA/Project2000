//==========================
// - FileName: ReNameDrawer.cs
// - Created: AjieloA
// - CreateTime: 2024-09-23 11:48:10
// - Email: 1758580256@qq.com
// - Description:
//==========================
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ReNameAttribute))]
public class ReNameDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {

        EditorGUI.PropertyField(position, property, new GUIContent((attribute as ReNameAttribute).mName), true);
    }
}
