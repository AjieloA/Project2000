//==========================
// - FileName: ReadOnlyDrawer.cs
// - Created: AjieloA
// - CreateTime: 2024-09-23 11:17:37
// - Email: 1758580256@qq.com
// - Description:
//==========================
using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(DisplayOnlyAttribute))]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, label, true);
    }
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        GUI.enabled = true;
    }
}