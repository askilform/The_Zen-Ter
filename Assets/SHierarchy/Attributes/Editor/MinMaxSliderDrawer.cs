#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

namespace Shadowprofile.Attributes.SHierarchy
{
    [CustomPropertyDrawer(typeof(MinMaxSliderAttribute))]
    public class MinMaxSliderDrawer : PropertyDrawer
    {
        private const float Padding = 5f;
        private const float FieldWidth = 50f;
        private const float SliderHeight = 18f; // Altura fixa para cada linha
        private const float TotalHeight = SliderHeight + Padding; // Altura total considerando o padding

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            MinMaxSliderAttribute slider = (MinMaxSliderAttribute)attribute;
            
            if (property.propertyType != SerializedPropertyType.Vector2)
            {
                EditorGUI.LabelField(position, label.text, "Use MinMaxSlider with Vector2.");
                return;
            }

            Vector2 range = property.vector2Value;

            // Ajuste responsivo dos elementos
            float labelWidth = EditorGUIUtility.labelWidth;
            float sliderWidth = position.width - labelWidth - FieldWidth * 2 - Padding * 3;

            // Rects responsivos
            Rect labelRect = new Rect(position.x, position.y, labelWidth, SliderHeight);
            Rect minFieldRect = new Rect(labelRect.xMax + Padding, position.y, FieldWidth, SliderHeight);
            Rect sliderRect = new Rect(minFieldRect.xMax + Padding, position.y, sliderWidth, SliderHeight);
            Rect maxFieldRect = new Rect(sliderRect.xMax + Padding, position.y, FieldWidth, SliderHeight);

            // Renderizar label
            EditorGUI.LabelField(labelRect, label);

            // Renderizar campo mínimo
            range.x = EditorGUI.FloatField(minFieldRect, range.x);
            range.x = Mathf.Clamp(range.x, slider.MinLimit, range.y);

            // Renderizar slider
            EditorGUI.MinMaxSlider(sliderRect, ref range.x, ref range.y, slider.MinLimit, slider.MaxLimit);

            // Renderizar campo máximo
            range.y = EditorGUI.FloatField(maxFieldRect, range.y);
            range.y = Mathf.Clamp(range.y, range.x, slider.MaxLimit);

            // Atualizar o valor do Vector2
            property.vector2Value = range;
        }

        // Método para definir a altura da propriedade
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return TotalHeight;
        }
    }
}
#endif
