using UnityEngine;
using UnityEditor;

/// <summary>
/// Custom property drawer for the Label attribute.
/// Renders the info text as a label inside a box above the property field.
/// </summary>
namespace Shadowprofile.Attributes.SHierarchy
{
    [CustomPropertyDrawer(typeof(LabelAttribute))]
    public class LabelDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            LabelAttribute labelAttribute = (LabelAttribute)attribute;
            GUIStyle labelStyle = EditorStyles.helpBox;
            labelStyle.wordWrap = true;
            float labelHeight = labelStyle.CalcHeight(new GUIContent(labelAttribute.InfoText), position.width);
            Rect boxRect = new Rect(position.x, position.y, position.width, labelHeight);
            EditorGUI.HelpBox(boxRect, labelAttribute.InfoText, MessageType.Info);
            Rect fieldRect = new Rect(position.x, position.y + labelHeight + EditorGUIUtility.standardVerticalSpacing, position.width, position.height - labelHeight - EditorGUIUtility.standardVerticalSpacing);
            EditorGUI.PropertyField(fieldRect, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            LabelAttribute labelAttribute = (LabelAttribute)attribute;
            GUIStyle labelStyle = EditorStyles.helpBox;
            labelStyle.wordWrap = true;
            float labelHeight = labelStyle.CalcHeight(new GUIContent(labelAttribute.InfoText), EditorGUIUtility.currentViewWidth);
            return labelHeight + EditorGUIUtility.standardVerticalSpacing + EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}
