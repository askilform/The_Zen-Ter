using UnityEditor;
using UnityEngine;
namespace  Shadowprofile.Attributes.SHierarchy
{
[CustomPropertyDrawer(typeof(ShowIfAttribute))]
public class ShowIfDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute showIf = (ShowIfAttribute)attribute;
        SerializedProperty conditionProp = property.serializedObject.FindProperty(showIf.ConditionProperty);
        
        if (conditionProp != null && conditionProp.boolValue)
        {
            return base.GetPropertyHeight(property, label);
        }

        return 0f;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ShowIfAttribute showIf = (ShowIfAttribute)attribute;
        SerializedProperty conditionProp = property.serializedObject.FindProperty(showIf.ConditionProperty);

        if (conditionProp != null && conditionProp.boolValue)
        {
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}}