//==========================
// - FileName: SerializableQueueDrawer.cs
// - Created: AjieloA
// - CreateTime: 2024-09-24 14:26:10
// - Email: 1758580256@qq.com
// - Description:
//==========================
using UnityEditor;
using UnityEngine;
[CustomPropertyDrawer(typeof(SerializableQueue), true)]
public class SerializableQueueDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        //GUI.enabled = false;
        EditorGUI.PropertyField(position, property, label, true);
        //GUI.enabled = true;

    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return EditorGUI.GetPropertyHeight(property, true);
    }
}
