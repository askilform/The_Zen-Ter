using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Shadowprofile.Utils.SHierarchy;

namespace Shadowprofile.SHierarchy
{
    public class FloatingInspectorWindow : EditorWindow
    {
        private static CustomIconData s_CustomIconData;

        private List<Component> m_Targets;
        private VisualElement m_RootElement;
        private ScrollView m_ScrollView;

        [MenuItem("Window/Floating Inspector")]
        public static void ShowWindow()
        {
            var window = GetWindow<FloatingInspectorWindow>();
            window.titleContent = new GUIContent("Quick Inspector", EditorGUIUtility.IconContent("d_Settings Icon").image);
            window.Show();
        }

        private void CreateGUI()
        {
            m_RootElement = rootVisualElement;

            m_ScrollView = new ScrollView
            {
                name = "scrollView"
            };
            m_ScrollView.AddToClassList("scroll-view");
            m_RootElement.Add(m_ScrollView);

            var styleSheet = ShadowProfileUtils.GetAssetFromRelativePath<StyleSheet>("SHierarchy/Editor/Layout", "FloatingInspectorWindow");
            if (styleSheet != null)
            {
                m_RootElement.styleSheets.Add(styleSheet);
            }

            minSize = new Vector2(350, 10);

            rootVisualElement.RegisterCallback<GeometryChangedEvent>(_ => UpdateWindowSize());
        }

        public void SetTargets(Component[] newTargets)
        {
            s_CustomIconData = ShadowProfileUtils.GetAssetFromRelativePath<CustomIconData>("SHierarchy/Editor", "CustomIconData");
            m_Targets = new List<Component>(newTargets);

            m_ScrollView.Clear();

            foreach (var target in m_Targets)
            {
                var editorElement = new VisualElement();
                editorElement.AddToClassList("component-box");

                var componentType = target.GetType();

                var header = new VisualElement();
                header.AddToClassList("component-header");

                var icon = GetComponentIcon(componentType);
                var iconImage = new Image { image = icon };
                iconImage.AddToClassList("component-icon");

                var componentTitle = new Label(componentType.Name);
                componentTitle.AddToClassList("component-title");

                header.Add(iconImage);
                header.Add(componentTitle);

                editorElement.Add(header);

                var editor = Editor.CreateEditor(target);
                var inspectorContainer = new IMGUIContainer(() =>
                {
                    if (editor == null) return;
                    GUILayout.BeginVertical();
                    editor.OnInspectorGUI();
                    GUILayout.EndVertical();
                });
                inspectorContainer.style.flexGrow = 1;
                inspectorContainer.style.flexShrink = 1;
                inspectorContainer.focusable = true;
                inspectorContainer.RegisterCallback<MouseDownEvent>(evt =>
                {
                    if (evt.button == 0) inspectorContainer.Focus();
                });

                inspectorContainer.AddToClassList("inspector-container");

                editorElement.Add(inspectorContainer);
                m_ScrollView.Add(editorElement);
            }

            UpdateWindowSize();
        }

        private Texture GetComponentIcon(Type componentType)
        {
            Texture2D icon = null;

            if (s_CustomIconData != null)
            {
                var iconPaths = s_CustomIconData.GetIconPaths();
                icon = iconPaths.TryGetValue(componentType.Name, out var iconPath) ?
                    AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath) :
                    AssetPreview.GetMiniTypeThumbnail(componentType);
            }

            return icon != null ? icon : EditorGUIUtility.IconContent("cs Script Icon").image;
        }

        private void UpdateWindowSize()
        {
            if (m_ScrollView == null) return;

            m_ScrollView.style.height = StyleKeyword.Auto;

            var totalHeight = m_ScrollView.Children().Sum(child => child.resolvedStyle.height + child.resolvedStyle.marginTop + child.resolvedStyle.marginBottom);

            var minHeight = Mathf.Max(totalHeight, 10);
            minHeight = Mathf.Min(minHeight, 755);

            m_ScrollView.style.height = minHeight;

            minSize = new Vector2(350, minHeight);
            maxSize = new Vector2(350, minHeight);
        }

        private void OnDestroy()
        {
            rootVisualElement.Clear();
        }

    }
}