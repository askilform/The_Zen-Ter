using System;
using System.Collections.Generic;
using System.Linq;
using Shadowprofile.SHierarchy;
using UnityEditor;
using UnityEngine;

namespace Shadowprofile.SHierarchy
{
    public class ComponentListPopupWindow : EditorWindow
    {
        private Dictionary<Type, List<Component>> components;
        private GameObject sourceObject;
        private Vector2 scrollPos;
        private static bool showNames = false;

        public static void Show(GameObject gameObject, Dictionary<Type, List<Component>> groupedComponents)
        {
            var window = CreateInstance<ComponentListPopupWindow>();
            window.titleContent = new GUIContent($"{gameObject.name} : Components");
            window.sourceObject = gameObject;
            window.components = groupedComponents;
            window.position = new Rect(Screen.width / 2, Screen.height / 2, 300, 300);
            window.maxSize = new Vector2(300, 300);
            window.minSize = new Vector2(300, 300);
            window.ShowUtility();
        }

        private void OnEnable()
        {
            showNames = EditorPrefs.GetBool("ShowIconName", false);
        }

        private void OnGUI()
        {
            GUILayout.Space(10);

            scrollPos = GUILayout.BeginScrollView(scrollPos, false, true);

            int columns = showNames ? 1 : 8;
            int count = 0;

            GUILayout.BeginHorizontal();
            foreach (var group in components.OrderBy(g => g.Key.Name))
            {
                var icon = CustomHierarchyEditor.GetComponentIcon(group.Key, group.Value);
                if (icon == null) continue;

                // GUIContent content = showNames
                //     ? new GUIContent(group.Key.Name, icon)
                //     : new GUIContent(icon, group.Key.Name);

                float width = showNames ? 280 : 30;
                float height = 30;
                //
                // if (GUILayout.Button(content, GUILayout.Width(width), GUILayout.Height(height)))
                // {
                //     CustomHierarchyEditor.OpenInspector(group.Value.ToArray());
                // }
                if (showNames)
                {
                    if (GUILayout.Button("", GUILayout.Width(width), GUILayout.Height(height)))
                    {
                        CustomHierarchyEditor.OpenInspector(group.Value.ToArray());
                    }

                    var rect = GUILayoutUtility.GetLastRect();

                    GUI.DrawTexture(new Rect(rect.x + 4, rect.y + 4, 20, 20), icon);

                    var labelStyle = new GUIStyle(EditorStyles.label)
                    {
                        alignment = TextAnchor.MiddleLeft,
                        fontSize = 12,
                        fontStyle = FontStyle.Bold,
                        wordWrap = true
                    };
                    GUI.Label(new Rect(rect.x + 30, rect.y, rect.width - 54, rect.height), group.Key.Name, labelStyle);
                }
                else
                {
                    if (GUILayout.Button(new GUIContent(icon, group.Key.Name), GUILayout.Width(width),
                            GUILayout.Height(height)))
                    {
                        CustomHierarchyEditor.OpenInspector(group.Value.ToArray());
                    }
                }

                count++;
                if (count % columns == 0)
                {
                    GUILayout.EndHorizontal();
                    GUILayout.BeginHorizontal();
                }
            }

            GUILayout.EndHorizontal();

            GUILayout.EndScrollView();
        }
    }
}