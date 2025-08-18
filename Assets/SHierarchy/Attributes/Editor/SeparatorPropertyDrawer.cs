using UnityEditor;
using UnityEngine;

namespace Shadowprofile.Attributes.SHierarchy
{
    [CustomPropertyDrawer(typeof(SeparatorAttribute))]
    public class SeparatorPropertyDrawer : PropertyDrawer
    {
        // Define o espaçamento acima e abaixo da linha
        private const float TopSpacing = 7f;
        private const float BottomSpacing = 12f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // A altura do separador permanece a mesma (apenas a espessura da linha e o espaçamento)
            SeparatorAttribute separator = (SeparatorAttribute)attribute;
            return base.GetPropertyHeight(property, label) + TopSpacing + BottomSpacing + separator.thickness;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            SeparatorAttribute separator = (SeparatorAttribute)attribute;
            Color originalColor = GUI.color;

            // Espaço acima do separador
            position.y += TopSpacing;

            // Se houver texto, desenha a barra com texto no meio
            if (!string.IsNullOrEmpty(separator.separatorTitle))
            {
                // Calcula a largura e altura do texto
                float textWidth = GUI.skin.label.CalcSize(new GUIContent(separator.separatorTitle)).x;
                float textHeight = GUI.skin.label.CalcSize(new GUIContent(separator.separatorTitle)).y;

                // Define a cor da linha (barra)
                GUI.color = new Color(0.8f, 0.8f, 0.8f, 1f);

                // A altura da barra é sempre 'separator.thickness', então o texto deve ser centralizado dentro dessa altura
                float verticalOffset = (separator.thickness - textHeight) / 2f;

                // Desenha a barra esquerda
                float leftWidth = (position.width - textWidth) / 2 - 5; // Largura da barra esquerda
                EditorGUI.DrawRect(new Rect(position.x, position.y, leftWidth, separator.thickness), GUI.color);

                Rect textPosition = new Rect(position.x + leftWidth + 5, position.y + verticalOffset, textWidth,
                    textHeight);
                GUI.Label(textPosition, separator.separatorTitle);

                float rightWidth = position.width - leftWidth - textWidth - 10; // Largura da barra direita
                EditorGUI.DrawRect(
                    new Rect(position.x + leftWidth + textWidth + 10, position.y, rightWidth, separator.thickness),
                    GUI.color);
            }
            else
            {
                // Se não houver texto, apenas desenha uma linha simples
                GUI.color = new Color(0.8f, 0.8f, 0.8f, 1f);
                EditorGUI.DrawRect(new Rect(position.x, position.y, position.width, separator.thickness), GUI.color);
            }

            // Volta para a cor original
            GUI.color = originalColor;

            // Ajuste a posição para renderizar a propriedade abaixo do separador com o espaço apropriado
            position.y += separator.thickness + BottomSpacing;
            EditorGUI.PropertyField(position, property, label, true);
        }
    }
}
