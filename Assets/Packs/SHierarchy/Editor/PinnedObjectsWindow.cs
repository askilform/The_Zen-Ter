using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;
using System.Linq;
using UnityEngine.SceneManagement;
using Shadowprofile.Utils.SHierarchy;

namespace Shadowprofile.SHierarchy
{
    public class PinnedObjectsWindow : EditorWindow
    {
        private static List<GameObject> pinnedObjects = new List<GameObject>();
        private static ScrollView scrollView = new ScrollView { style = { flexGrow = 1, backgroundColor = new Color(.2f, .2f, .2f, 1) } };
        private static readonly int MaxNameLength = 20;
        private static PinnedObjectsData pinnedData;

        [MenuItem("Window/Pin Folder")]
        public static void ShowWindow()
        {
            PinnedObjectsWindow wnd = GetWindow<PinnedObjectsWindow>();
            wnd.titleContent = new GUIContent("Pin Folder");
            wnd.minSize = new Vector2(250, 200);
            wnd.maxSize = new Vector2(250, 200);
            LoadPinnedObjects();
        }

        public void OnEnable()
        {
            LoadDataAsset();
            LoadPinnedObjects();
        }

        public static void Refresh()
        {
            LoadPinnedObjects();
            RefreshPinnedObjectsList();
        }

        private static void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            LoadPinnedObjects();
            RefreshPinnedObjectsList();
        }

        [InitializeOnLoadMethod]
        private static void RegisterPlayModeStateChangeCallback()
        {
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            SceneManager.sceneLoaded += OnSceneLoaded;
            EditorApplication.hierarchyChanged += Refresh;
        }

        private static void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredEditMode || state == PlayModeStateChange.EnteredPlayMode)
            {
                LoadPinnedObjects();
                RefreshPinnedObjectsList();
            }
        }

        public void CreateGUI()
        {
            var root = rootVisualElement;
            root.Clear();
            root.Add(scrollView);


            RefreshPinnedObjectsList();
        }

        private static void RefreshPinnedObjectsList()
        {
            scrollView.Clear();

            foreach (var obj in pinnedObjects.ToList())
            {
                if (obj == null)
                {
                    pinnedObjects.Remove(obj);
                    SavePinnedObjects();
                    continue;
                }

                string truncatedName = obj.name.Length > MaxNameLength ? obj.name.Substring(0, MaxNameLength) + "..." : obj.name;

                var itemButton = new Button(() =>
                {
                    Selection.activeGameObject = obj;
                    EditorGUIUtility.PingObject(obj);
                })
                {
                    style =
                    {
                        flexDirection = FlexDirection.Row,
                        marginTop = 5,
                        alignItems = Align.Center,
                        justifyContent = Justify.Center,
                        backgroundColor = new Color(0.13f, 0.13f, 0.13f, 1f),
                        paddingLeft = 3,
                        paddingRight = 3,
                        height = 24
                    }
                };
                scrollView.Add(itemButton);

                var icon = new Image
                {
                    image = EditorGUIUtility.IconContent("GameObject Icon").image,
                    style = { width = 16, height = 16, marginRight = 5 }
                };
                itemButton.Add(icon);

                var nameLabel = new Label(truncatedName)
                {
                    style = { flexGrow = 1, unityTextAlign = TextAnchor.MiddleLeft }
                };
                itemButton.Add(nameLabel);

                var unpinButton = new Button(() =>
                {
                    pinnedObjects.Remove(obj);
                    SavePinnedObjects();
                    RefreshPinnedObjectsList();
                })
                {
                    text = "X",
                    style = { width = 24, height = 24, backgroundColor = Color.clear }
                };
                itemButton.Add(unpinButton);
            }
        }

        public static bool IsPinned(GameObject obj)
        {
            return pinnedObjects.Contains(obj);
        }

        public static void PinObject(GameObject obj)
        {
            if (obj.scene.isLoaded)
            {
                if (!pinnedObjects.Contains(obj))
                {
                    pinnedObjects.Add(obj);
                    SavePinnedObjects();
                    if (HasOpenInstances<PinnedObjectsWindow>())
                    {
                        RefreshPinnedObjectsList();
                    }
                }
                else
                {
                    UnpinObject(obj);
                }
            }

        }


        public static void UnpinObject(GameObject obj)
        {
            pinnedObjects.Remove(obj);

            SavePinnedObjects();
            if (HasOpenInstances<PinnedObjectsWindow>())
            {
                RefreshPinnedObjectsList();
            }
        }

        public static void LoadDataAsset()
        {
            if (pinnedData == null)
            {
                pinnedData = ShadowProfileUtils.GetAssetFromRelativePath<PinnedObjectsData>("SHierarchy/Editor", "PinnedObjectsData");
          //      Debug.Log($"PinnedData {pinnedData}");
                if (pinnedData == null)
                {
            //        Debug.Log("Created New Asset");
                    pinnedData = CreateInstance<PinnedObjectsData>();
                    AssetDatabase.CreateAsset(pinnedData, $"{ShadowProfileUtils.GetFullPathFromRelativePath("SHierarchy/Editor")}/PinnedObjectsData.asset");
                    AssetDatabase.SaveAssets();
                }
            }
        }


        private static void SavePinnedObjects()
        {
            if (pinnedData == null) LoadDataAsset();

            List<string> objectIds = pinnedObjects
                .Where(obj => obj != null)
                .Select(obj => GlobalObjectId.GetGlobalObjectIdSlow(obj).ToString())
                .ToList();

            pinnedData.SetPinnedObjectsForScene(SceneManager.GetActiveScene().name, objectIds);

            EditorUtility.SetDirty(pinnedData);
            AssetDatabase.SaveAssets();
        }

        public static void LoadPinnedObjects()
        {
            if (pinnedData == null) LoadDataAsset();

            pinnedObjects.Clear();

            var objectIds = pinnedData.GetPinnedObjectsForScene(SceneManager.GetActiveScene().name);

            foreach (var id in objectIds)
            {
                if (GlobalObjectId.TryParse(id, out GlobalObjectId globalId))
                {
                    var obj = GlobalObjectId.GlobalObjectIdentifierToObjectSlow(globalId) as GameObject;
                    if (obj != null)
                    {
                        pinnedObjects.Add(obj);
                    }
                }
            }
        }


        [System.Serializable]
        private class GameObjectIDList
        {
            public int[] ids;
        }


        [InitializeOnLoad]
        public static class PinnedObjectsInjector
        {
            static PinnedObjectsInjector()
            {
                EditorApplication.delayCall += AddPinnedObjectsButtonToHierarchy;
            }


            private static void AddPinnedObjectsButtonToHierarchy()
            {
                var window = GetSceneHierarchyWindow();
                if (window != null)
                {
                    AddPinnedObjectsButton(window);
                }
            }

            private static void AddPinnedObjectsButton(EditorWindow window)
            {
                var existingButton = window.rootVisualElement.Q<Button>("pinnedObjectsButton");
                if (existingButton != null) return;
                Texture2D icon = ShadowProfileUtils.GetAssetFromRelativePath<Texture2D>("SHierarchy/Editor/Ico", "pinned");

                var iconImage = new Image { image = icon, style = { height = 18, width = 15 } };

                var pinnedObjectsButton = new Button(() =>
                {
                    PinnedObjectsWindow.ShowWindow();
                })
                {
                    style = { width = 17, height = 18, marginLeft = 32, alignItems = Align.Center, justifyContent = Justify.Center, backgroundColor = Color.clear },
                    name = "pinnedObjectsButton"
                };
                pinnedObjectsButton.Add(iconImage);
                window.rootVisualElement.Add(pinnedObjectsButton);
            }

            private static EditorWindow GetSceneHierarchyWindow()
            {
                var type = typeof(EditorWindow).Assembly.GetType("UnityEditor.SceneHierarchyWindow");
                return type != null ? EditorWindow.GetWindow(type) : null;
            }
        }

    }
}
