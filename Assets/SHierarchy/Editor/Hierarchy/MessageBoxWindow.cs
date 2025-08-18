using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Shadowprofile.Utils.SHierarchy;
namespace Shadowprofile.SHierarchy
{
    public class MessageBoxWindow : EditorWindow
    {
        private List<string> m_Warnings;
        private List<string> m_Errors;
        private Label m_MessageLabel;

        private Label m_TitleLabel;
        private Label m_ErrorsLabel;
        private Label m_WarningsLabel;

        private VisualElement m_ErrorsContainer;
        private VisualElement m_WarningsContainer;
        private ScrollView m_ScrollView;

        public static void ShowWindow(List<string> warnings, List<string> errors)
        {
            var window = GetWindow<MessageBoxWindow>("GameObject Issues");
            window.Init(warnings, errors);
            window.minSize = new Vector2(320, 250);
            window.maxSize = new Vector2(320, 250);
            window.Show();
        }

        public void Init(List<string> warnings, List<string> errors)
        {
            m_Warnings = warnings;
            m_Errors = errors;

            UpdateMessage();
        }

        private void UpdateMessage()
        {
            if (m_ErrorsContainer == null || m_WarningsContainer == null)
            {
                Debug.LogError("UI elements are not initialized.");
                return;
            }

            m_ErrorsContainer.Clear();
            m_WarningsContainer.Clear();

            var errorIcon = EditorGUIUtility.IconContent("console.erroricon").image as Texture2D;
            var warningIcon = EditorGUIUtility.IconContent("console.warnicon").image as Texture2D;

            if (m_Warnings.Count == 0)
            {
                var noWarningsElement = CreatePlaceholderMessage("No warnings found", warningIcon, "warningItem");
                m_WarningsContainer.Add(noWarningsElement);
            }
            if (m_Errors.Count == 0)
            {
                var noErrorsElement = CreatePlaceholderMessage("No errors found", errorIcon, "errorItem");
                m_ErrorsContainer.Add(noErrorsElement);
            }

            foreach (var error in m_Errors)
            {
                var errorElement = new VisualElement();
                errorElement.AddToClassList("messageBox");
                errorElement.AddToClassList("errorItem");

                var errorImage = new Image { image = errorIcon };
                errorImage.AddToClassList("icon");

                var errorText = new Label(error);
                errorText.AddToClassList("messageText");

                errorElement.Add(errorImage);
                errorElement.Add(errorText);

                m_ErrorsContainer.Add(errorElement);
            }

            foreach (var warning in m_Warnings)
            {
                var warningElement = new VisualElement();
                warningElement.AddToClassList("messageBox");
                warningElement.AddToClassList("warningItem");

                var warningImage = new Image { image = warningIcon };
                warningImage.AddToClassList("icon");

                var warningText = new Label(warning);
                warningText.AddToClassList("messageText");

                warningElement.Add(warningImage);
                warningElement.Add(warningText);

                m_WarningsContainer.Add(warningElement);
            }

        }

        private VisualElement CreatePlaceholderMessage(string message, Texture2D icon, string itemClass)
        {
            var placeholder = new VisualElement();
            placeholder.AddToClassList("messageBox");
            placeholder.AddToClassList(itemClass);

            var iconElement = new Image { image = icon };
            iconElement.AddToClassList("icon");

            var textElement = new Label(message);
            textElement.AddToClassList("messageText");

            placeholder.Add(iconElement);
            placeholder.Add(textElement);

            return placeholder;
        }

        private void OnEnable()
        {
            var styleSheet = ShadowProfileUtils.GetAssetFromRelativePath<StyleSheet>("SHierarchy/Editor/Layouts/", "MessageBoxWindow");
            if (styleSheet == null) return;

            var root = rootVisualElement;

            root.styleSheets.Add(styleSheet);

            m_ScrollView = new ScrollView();
            m_ScrollView.AddToClassList("scrollView");

            var titleElement = new Label("");
            titleElement.AddToClassList("title");
            m_ScrollView.Add(titleElement);

            m_ErrorsContainer = new VisualElement();
            m_ErrorsContainer.AddToClassList("errorsContainer");
            m_ScrollView.Add(m_ErrorsContainer);

            m_WarningsContainer = new VisualElement();
            m_WarningsContainer.AddToClassList("warningsContainer");
            m_ScrollView.Add(m_WarningsContainer);

            root.Add(m_ScrollView);
        }
    }
}