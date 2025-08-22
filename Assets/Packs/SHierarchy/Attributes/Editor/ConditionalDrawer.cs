#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Shadowprofile.Attributes.SHierarchy
{
    [CustomPropertyDrawer(typeof(DisableIfAttribute))]
    [CustomPropertyDrawer(typeof(EnableIfAttribute))]
    public class ConditionalDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var conditionAttribute = (PropertyAttribute)attribute;
            string condition = (conditionAttribute is DisableIfAttribute)
                ? ((DisableIfAttribute)conditionAttribute).Condition
                : ((EnableIfAttribute)conditionAttribute).Condition;

            SerializedProperty conditionProperty = property.serializedObject.FindProperty(condition);

            if (conditionProperty != null)
            {
                bool enabled = conditionProperty.boolValue;
                if (attribute is DisableIfAttribute) enabled = !enabled;

                GUI.enabled = enabled;
                EditorGUI.PropertyField(position, property, label, true);
                GUI.enabled = true;
            }
            else
            {
                EditorGUI.LabelField(position, label.text, $"Condition '{condition}' not found.");
            }
        }
    }
}
#endif