using UnityEditor;
using UnityEngine;

namespace Shadowprofile.Attributes.SHierarchy
{
    [CustomPropertyDrawer(typeof(HideIfAttribute))]
    public class HideIfDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            HideIfAttribute hideIf = (HideIfAttribute)attribute;
            SerializedProperty conditionProp = property.serializedObject.FindProperty(hideIf.ConditionProperty);

            if (conditionProp != null && conditionProp.boolValue)
            {
                return 0f;
            }

            return base.GetPropertyHeight(property, label);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            HideIfAttribute hideIf = (HideIfAttribute)attribute;
            SerializedProperty conditionProp = property.serializedObject.FindProperty(hideIf.ConditionProperty);

            if (conditionProp != null && conditionProp.boolValue)
            {
                return;
            }

            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}