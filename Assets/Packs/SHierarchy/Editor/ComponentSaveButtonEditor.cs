using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using System.Collections.Generic;
using System;
using System.Linq;

namespace Shadowprofile.SHierarchy
{
    [InitializeOnLoad]
    public static class ComponentSaveButtonEditor
    {
        private static Dictionary<string, SerializedObject> playModeBackups = new();

        private static Texture2D i_pinIcon;

        private static Texture2D s_pinIcon
        {
            get
            {
                if (i_pinIcon == null) i_pinIcon = EditorGUIUtility.IconContent("d_SaveAs").image as Texture2D;
                return i_pinIcon;
            }
        }
        
        private static bool m_showRuntimeSave = true;

        static ComponentSaveButtonEditor()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            EditorApplication.update += TryInjectButtons;
        }


        private static void TryInjectButtons()
        {
            m_showRuntimeSave = EditorPrefs.GetBool("ShowRuntimeSave");
            if (!m_showRuntimeSave) return;
            var inspectorType = typeof(Editor).Assembly.GetType("UnityEditor.InspectorWindow");

            var inspectors = Resources.FindObjectsOfTypeAll(inspectorType);
            if (inspectors.Length > 1) return;
            foreach (var inspector in inspectors)
            {
                
                var rootElement = (VisualElement)inspectorType.GetProperty("rootVisualElement")?.GetValue(inspector);
                if (rootElement == null) continue;

                // Ugly way to get the children container
                var scrollView = rootElement.Q<ScrollView>();
                var editorElements = scrollView.Q<VisualElement>("unity-content-container")
                    .Q<VisualElement>("", "unity-inspector-editors-list");
                if (!editorElements.Children().Any()) break;
                var children = editorElements.Children();
             
                var containers = new List<IMGUIContainer>();

                // var firstChild = true;
                //
                // foreach (var child in children)
                // {
                //     if (firstChild)
                //     {
                //         firstChild = false;
                //         continue;
                //     }
                //
                //     containers.Add(child.Q<IMGUIContainer>());
                // }
                foreach (var child in children.Skip(1)) // Pular o primeiro (usualmente padrão)
                {
                    var container = child.Q<IMGUIContainer>();
                    if (container != null)
                        containers.Add(container);
                }

                var editors = GetEditorsFromTracker(inspector);
                if (editors == null || editors.Length <= 1)
                {
            //        Debug.LogWarning("Nenhum editor adicional encontrado ou apenas o padrão.");
                    continue;
                }

                if (containers.Count != editors.Length - 1)
                {
              //      Debug.LogWarning("Desalinhamento entre o número de containers e editores. Interrompendo execução.");
                    break;
                }

             
                // var editors = GetEditorsFromTracker(inspector);
                // if (editors == null || editors.Length <= 0)
                // {
                //     continue;
                // }
                //
                //
                //
                // if (editors.Length <= 2) break;
                for (int i = 0; i < containers.Count; i++)
                {
                    var container = containers[i];

                    var editor = editors[i + 1];

                    if (container.name.EndsWith("Injected")) continue;
                    container.name = container.name + "Injected";
                    var originalHandler = container.onGUIHandler;
                    container.onGUIHandler = () =>
                    {
                        var targetComponent = editor.target;

                        var rect = EditorGUILayout.GetControlRect(false, 0f);
                        rect.height = 16;
                        rect.y += 5.15f;
                        rect.x = rect.width - 76;
                        rect.width = 17;

                        Color hoverColor = new Color(0.23f, 0.23f, 0.23f, 1.0f);
                        Color clearColor = new Color(0, 0, 0, 0);

                        bool isHovering = rect.Contains(Event.current.mousePosition);

                        EditorGUI.DrawRect(rect, isHovering ? hoverColor : clearColor);

                        var s_savButtonStyle = new GUIStyle(GUIStyle.none)
                        {
                            alignment = TextAnchor.MiddleCenter,
                            imagePosition = ImagePosition.ImageOnly,
                            contentOffset = new Vector2(0, 0f),

                            border = new RectOffset(0, 0, 0, 0),
                            padding = new RectOffset(0, 0, 0, 0),
                            margin = new RectOffset(0, 0, 0, 0),
                            overflow = new RectOffset(0, 0, 0, 0),

                            normal = { background = null },
                            hover = { background = null },
                            active = { background = null },
                        };

                        float scaleFactor = 0.9f;
                        float imageWidth = rect.width * scaleFactor;
                        float imageHeight = rect.height * scaleFactor;

                        Rect imageRect = new Rect(
                            rect.x + (rect.width - imageWidth) / 2,
                            rect.y + (rect.height - imageHeight) / 2,
                            imageWidth,
                            imageHeight
                        );

                        GUIContent buttonContent = new GUIContent(s_pinIcon, "Save Changes");
                        if (GUI.Button(rect, buttonContent, s_savButtonStyle))
                        {
                            SaveChangesToEditor(targetComponent);
                            Event.current.Use();
                        }

                        originalHandler?.Invoke();

                        GUI.Button(rect, GUIContent.none, s_savButtonStyle);
                        if (isHovering)
                        {
                            EditorGUI.DrawRect(rect, hoverColor);
                        }

                        GUI.DrawTexture(imageRect, s_pinIcon, ScaleMode.ScaleToFit);
                    };
                }

                return;
            }
        }

        private static Editor[] GetEditorsFromTracker(UnityEngine.Object inspector)
        {
            try
            {
                var trackerField = inspector.GetType().GetField("m_Tracker",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic);
                var tracker = trackerField?.GetValue(inspector);

                if (tracker == null)
                {
                    Debug.LogWarning("Unable to access m_Tracker from Inspector.");
                    return null;
                }

                var activeEditorsProperty = tracker.GetType().GetProperty("activeEditors",
                    System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public);
                return activeEditorsProperty?.GetValue(tracker) as Editor[];
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public static void SaveChangesToEditor(UnityEngine.Object target)
        {
            if (!Application.isPlaying)
            {
                Debug.LogWarning("Not in PlayMode. Changes were not saved.");
                return;
            }

            string targetId = GlobalObjectId.GetGlobalObjectIdSlow(target).ToString();
            if (!playModeBackups.ContainsKey(targetId))
            {
                var serializedObj = new SerializedObject(target);
                playModeBackups[targetId] = serializedObj;
                Debug.Log(
                    $"Temporary changes saved for component {target.GetType().Name} (ID: {targetId}) during PlayMode.");
            }
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode && playModeBackups.Count > 0)
            {
                foreach (var backup in playModeBackups)
                {
                    var targetId = backup.Key;
                    var serializedBackup = backup.Value;
                    var targetObject = serializedBackup.targetObject as Component;

                    if (targetObject != null)
                    {
                        var editorSerializedObject = new SerializedObject(targetObject);
                        RestoreSerializedProperties(serializedBackup, editorSerializedObject);
                        editorSerializedObject.ApplyModifiedProperties();
                    }
                }

                playModeBackups.Clear();
            }
        }

        private static void RestoreSerializedProperties(SerializedObject source, SerializedObject destination)
        {
            var property = source.GetIterator();
            property.Next(true);

            while (property.NextVisible(true))
            {
                var destProperty = destination.FindProperty(property.propertyPath);
                if (destProperty != null)
                {
                    destProperty.serializedObject.CopyFromSerializedProperty(property);
                }
            }
        }
    }
}