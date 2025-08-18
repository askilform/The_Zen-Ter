using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Shadowprofile.SHierarchy
{
    public class FavoritesWindow : EditorWindow
    {
        private Dictionary<string, List<string>> categorizedFavorites = new();
        private List<string> categories = new() { "Default" };
        private int selectedCategoryIndex;
        private string newCategoryName = "Cat. Name";
        private string searchQuery = "";
        private Dictionary<string, string> categoryIcons = new();
        private double lastClickTime;
        private const float doubleClickTime = 0.3f;
        private bool isCategoryContainerCollapsed;

        private VisualElement topbar;
        private ScrollView categoryContainer;
        private ScrollView favoritesContainer;

        [MenuItem("Window/sFavorite")]
        public static void ShowWindow()
        {
            var window = GetWindow<FavoritesWindow>();
            window.titleContent = new GUIContent("sFavorite", EditorGUIUtility.IconContent("d_Favorite Icon").image);
        }

        private void OnEnable()
        {
            LoadFavorites();
            CreateUI();
        }

        private void OnDisable()
        {
            SaveFavorites();
        }

        private void CreateUI()
        {
            var root = rootVisualElement;
            root.Clear();
            root.style.flexDirection = FlexDirection.Column;

            topbar = new VisualElement();
            topbar.style.flexDirection = FlexDirection.Row;
            topbar.style.paddingBottom = 5;
            topbar.style.marginTop = 5;
            root.Add(topbar);

            RefreshTopBar();

            var mainContainer = new VisualElement();
            mainContainer.style.flexDirection = FlexDirection.Row;
            mainContainer.style.flexGrow = 1;
            root.Add(mainContainer);

            categoryContainer = new ScrollView();
            categoryContainer.style.width = 200;
            categoryContainer.style.marginRight = 5;
            categoryContainer.style.flexShrink = 0;
            mainContainer.Add(categoryContainer);

            var divider = new VisualElement();
            divider.style.width = 2;
            divider.style.backgroundColor = new Color(0.15f, .15f, .15f);
            divider.style.marginRight = 5;
            mainContainer.Add(divider);

            favoritesContainer = new ScrollView();
            favoritesContainer.style.flexGrow = 1;
            favoritesContainer.style.overflow = Overflow.Hidden;
            mainContainer.Add(favoritesContainer);

            RefreshCategoryArea();
            RefreshFavoritesArea();

            favoritesContainer.RegisterCallback<DragUpdatedEvent>(HandleDragAndDrop);
            favoritesContainer.RegisterCallback<DragPerformEvent>(HandleDragAndDrop);
        }


        private void RefreshTopBar()
        {
            topbar.Clear();
            var showHideButton = new Button(() =>
            {
                isCategoryContainerCollapsed = !isCategoryContainerCollapsed;
                RefreshTopBar();
                categoryContainer.style.display = isCategoryContainerCollapsed ? DisplayStyle.None : DisplayStyle.Flex;
            });
            showHideButton.text = isCategoryContainerCollapsed ? "▶" : "◀";
            showHideButton.style.width = 20;
            showHideButton.style.height = 20;
            showHideButton.style.backgroundColor = Color.clear;
            topbar.Add(showHideButton);

            var divider = new VisualElement();
            divider.style.width = 2;
            divider.style.backgroundColor = new Color(0.15f, .15f, .15f);
            divider.style.marginRight = 5;
            topbar.Add(divider);

            var searchField = new TextField { value = searchQuery };
            searchField.style.flexGrow = 1;
            searchField.style.marginLeft = 5;
            searchField.style.position = Position.Relative;

            var visualInput = searchField.Q("unity-text-input");

            var searchIcon = new Image { image = EditorGUIUtility.IconContent("Search Icon").image };
            searchIcon.style.width = 18;
            searchIcon.style.height = 18;
            searchIcon.style.position = Position.Absolute;
            searchIcon.style.left = 5;
            searchIcon.style.top = 3;
            visualInput.parent.Add(searchIcon);

            visualInput.style.paddingLeft = 25;

            var clearButtonTextField = new Button(() =>
            {
                searchField.value = "";
                RefreshFavoritesArea();
            });

            var iconDeleteTextField = EditorGUIUtility.IconContent("d_Grid.EraserTool").image as Texture2D;
            clearButtonTextField.style.width = 20;
            clearButtonTextField.style.height = 20;
            clearButtonTextField.style.position = Position.Absolute;
            clearButtonTextField.style.right = 0;
            clearButtonTextField.style.top = -2;
            clearButtonTextField.style.backgroundColor = Color.clear;
            clearButtonTextField.style.borderBottomColor = Color.clear;
            clearButtonTextField.style.borderTopColor = Color.clear;
            clearButtonTextField.style.borderRightColor = Color.clear;
            clearButtonTextField.style.borderLeftColor = Color.clear;
            clearButtonTextField.style.visibility = Visibility.Hidden;
            clearButtonTextField.style.backgroundImage = new StyleBackground(iconDeleteTextField);
            visualInput.parent.Add(clearButtonTextField);

            void UpdateClearButtonVisibility()
            {
                clearButtonTextField.style.visibility =
                    string.IsNullOrEmpty(searchField.value) ? Visibility.Hidden : Visibility.Visible;
            }

            searchField.RegisterValueChangedCallback(evt =>
            {
                searchQuery = evt.newValue;
                RefreshFavoritesArea();
                UpdateClearButtonVisibility();
            });

            UpdateClearButtonVisibility();

            topbar.Add(searchField);

            var clearButton = new Button(() =>
            {
                if (categories.Count > selectedCategoryIndex)
                {
                    categorizedFavorites[categories[selectedCategoryIndex]].Clear();
                    RefreshFavoritesArea();
                }
            })
            {
                text = "Clear"
            };
            clearButton.style.width = 50;
            clearButton.style.marginLeft = 5;
            topbar.Add(clearButton);
        }

        private void RefreshCategoryArea()
        {
            categoryContainer.Clear();

            for (var i = 0; i < categories.Count; i++)
            {
                var index = i;
                var categoryButton = new VisualElement();
                categoryButton.style.flexDirection = FlexDirection.Row;
                categoryButton.style.height = 40;
                categoryButton.style.justifyContent = Justify.SpaceBetween;
                categoryButton.style.alignItems = Align.Center;

                if (i == selectedCategoryIndex)
                {
                    categoryButton.style.backgroundColor = SetCategoryColor(categories[i]);
                    categoryButton.style.color = Color.white;
                }
                else
                {
                    categoryButton.style.backgroundColor = Color.clear;
                    categoryButton.style.color = Color.white;
                }

                var icon = new Image { image = SetCategoryIcon(categories[i]) };
                icon.style.width = 30;
                icon.style.height = 30;
                icon.style.marginLeft = 5;

                categoryButton.Add(icon);

                var categoryName = new Button(() =>
                {
                    selectedCategoryIndex = index;
                    RefreshCategoryArea();
                    RefreshFavoritesArea();
                });
                categoryName.text = categories[i];
                categoryName.style.flexGrow = 1;
                categoryName.style.height = 35;
                categoryName.style.textOverflow = TextOverflow.Ellipsis;
                categoryName.style.unityTextAlign = TextAnchor.MiddleLeft;
                categoryName.style.backgroundColor = Color.clear;
                categoryName.style.borderBottomColor = Color.clear;
                categoryName.style.borderTopColor = Color.clear;
                categoryName.style.borderRightColor = Color.clear;
                categoryName.style.borderLeftColor = Color.clear;
                categoryButton.Add(categoryName);

                var closeButton = new Button(() => { RemoveCategory(index); });
                var iconDelete = EditorGUIUtility.IconContent("d_CollabDeleted Icon").image as Texture2D;
                closeButton.style.backgroundColor = Color.clear;
                closeButton.style.borderBottomColor = Color.clear;
                closeButton.style.borderTopColor = Color.clear;
                closeButton.style.borderRightColor = Color.clear;
                closeButton.style.borderLeftColor = Color.clear;
                closeButton.style.backgroundImage = new StyleBackground(iconDelete);
                closeButton.style.width = 15;
                closeButton.style.height = 15;
                closeButton.style.marginRight = 5;
                categoryButton.Add(closeButton);

                categoryContainer.Add(categoryButton);
            }

            var addCategoryRow = new VisualElement();
            addCategoryRow.style.flexDirection = FlexDirection.Row;
            addCategoryRow.style.marginTop = 5;
            categoryContainer.Add(addCategoryRow);

            var categoryNameField = new TextField { value = newCategoryName };
            categoryNameField.style.flexGrow = 1;
            categoryNameField.style.marginRight = 5;
            categoryNameField.RegisterValueChangedCallback(evt => newCategoryName = evt.newValue);
            addCategoryRow.Add(categoryNameField);

            var addCategoryButton = new Button(() =>
            {
                AddCategory(newCategoryName);
                RefreshCategoryArea();
            });

            var iconAddCategory = EditorGUIUtility.IconContent("d_CreateAddNew").image as Texture2D;
            addCategoryButton.style.backgroundColor = new Color(0.33f, 0.33f, 0.33f);
            addCategoryButton.style.borderBottomColor = Color.clear;
            addCategoryButton.style.borderTopColor = Color.clear;
            addCategoryButton.style.borderRightColor = Color.clear;
            addCategoryButton.style.borderLeftColor = Color.clear;
            addCategoryButton.style.backgroundImage = new StyleBackground(iconAddCategory);
            addCategoryButton.style.height = 20;
            addCategoryButton.style.width = 20;

            var hoverBackgroundColor = new Color(0.45f, 0.45f, 0.45f);
            var normalBackgroundColor = Color.clear;

            addCategoryButton.RegisterCallback<MouseEnterEvent>(_ =>
            {
                addCategoryButton.style.backgroundColor = hoverBackgroundColor;
                addCategoryButton.style.height = 20;
                addCategoryButton.style.width = 20;
            });

            addCategoryButton.RegisterCallback<MouseLeaveEvent>(_ =>
            {
                addCategoryButton.style.backgroundColor = normalBackgroundColor;
                addCategoryButton.style.height = 15;
                addCategoryButton.style.width = 15;
            });
            addCategoryRow.Add(addCategoryButton);
        }

        private void RefreshFavoritesArea()
        {
            favoritesContainer.Clear();

            if (categories.Count == 0 || selectedCategoryIndex < 0 || selectedCategoryIndex >= categories.Count)
            {
                favoritesContainer.Add(new Label("No Categories available."));
                return;
            }

            var category = categories[selectedCategoryIndex];
            var favoriteItemPaths = categorizedFavorites[category];
            var filteredItems = string.IsNullOrEmpty(searchQuery)
                ? favoriteItemPaths
                : favoriteItemPaths.Where(path =>
                        Path.GetFileNameWithoutExtension(path).Contains(searchQuery, StringComparison.OrdinalIgnoreCase))
                    .ToList();

            if (filteredItems.Count == 0)
            {
                favoritesContainer.Add(new Label("No items found."));
                return;
            }

            var itemContainer = new VisualElement();
            itemContainer.style.flexDirection = FlexDirection.Row;
            itemContainer.style.flexWrap = Wrap.Wrap;
            favoritesContainer.Add(itemContainer);

            foreach (var path in filteredItems)
            {
                var itemElement = new VisualElement();
                itemElement.style.width = 80;
                itemElement.style.marginRight = 10;
                itemElement.style.marginBottom = 10;

                var item = AssetDatabase.LoadAssetAtPath<Object>(path);
                if (item == null) continue;

                var preview = new Image { image = GetAssetPreview(item) };
                preview.style.width = 60;
                preview.style.height = 60;
                preview.style.alignSelf = Align.Center;
                itemElement.Add(preview);

                var labelAndButtonContainer = new VisualElement();
                labelAndButtonContainer.style.flexDirection = FlexDirection.Row;
                labelAndButtonContainer.style.justifyContent = Justify.SpaceBetween;
                labelAndButtonContainer.style.alignItems = Align.Center;

                var itemName = new Label(item.name);
                itemName.style.maxWidth = 60;
                itemName.style.unityTextAlign = TextAnchor.MiddleCenter;
                itemName.style.whiteSpace = WhiteSpace.NoWrap;
                itemName.style.overflow = Overflow.Hidden;
                itemName.style.textOverflow = TextOverflow.Ellipsis;
                itemName.style.flexGrow = 1;
                itemElement.Add(itemName);

                var removeButton = new Button(() => { RemoveFavoriteItem(category, path); });
                var iconDelete = EditorGUIUtility.IconContent("d_CollabDeleted Icon").image as Texture2D;
                removeButton.style.backgroundColor = Color.clear;
                removeButton.style.borderBottomColor = Color.clear;
                removeButton.style.borderTopColor = Color.clear;
                removeButton.style.borderRightColor = Color.clear;
                removeButton.style.borderLeftColor = Color.clear;
                removeButton.style.backgroundImage = new StyleBackground(iconDelete);
                removeButton.style.width = 15;
                removeButton.style.height = 15;
                removeButton.style.marginLeft = 5;
                labelAndButtonContainer.Add(itemName);
                labelAndButtonContainer.Add(removeButton);

                itemElement.Add(labelAndButtonContainer);
                itemElement.RegisterCallback<MouseDownEvent>(_ => HandleItemClick(item));
                itemContainer.Add(itemElement);
            }
        }

        private void HandleItemClick(Object item)
        {
            if (EditorApplication.timeSinceStartup - lastClickTime < doubleClickTime)
            {
                AssetDatabase.OpenAsset(item);
            }
            else
            {
                Selection.activeObject = item;
                EditorGUIUtility.PingObject(item);
            }

            lastClickTime = EditorApplication.timeSinceStartup;
        }

        private void HandleDragAndDrop(DragUpdatedEvent evt)
        {
            DragAndDrop.visualMode = DragAndDropVisualMode.Copy;
            evt.StopPropagation();
        }

        private void HandleDragAndDrop(DragPerformEvent evt)
        {
            if (selectedCategoryIndex >= 0 && selectedCategoryIndex < categories.Count)
            {
                DragAndDrop.AcceptDrag();
                foreach (var draggedObject in DragAndDrop.objectReferences)
                {
                    var path = AssetDatabase.GetAssetPath(draggedObject);
                    if (!categorizedFavorites[categories[selectedCategoryIndex]].Contains(path))
                    {
                        categorizedFavorites[categories[selectedCategoryIndex]].Add(path);
                    }
                }

                RefreshFavoritesArea();
            }

            evt.StopPropagation();
        }

        private void RemoveCategory(int index)
        {
            if (index < 0 || index >= categories.Count) return;

            var category = categories[index];
            categorizedFavorites.Remove(category);
            categories.RemoveAt(index);
            selectedCategoryIndex = Mathf.Clamp(selectedCategoryIndex, 0, categories.Count - 1);
            RefreshCategoryArea();
            RefreshFavoritesArea();
        }

        private void RemoveFavoriteItem(string category, string path)
        {
            if (!categorizedFavorites.ContainsKey(category)) return;
            categorizedFavorites[category].Remove(path);
            RefreshFavoritesArea();
        }

        private void AddCategory(string categoryName)
        {
            if (categorizedFavorites.ContainsKey(categoryName)) return;
            categorizedFavorites[categoryName] = new List<string>();
            categories.Add(categoryName);
            newCategoryName = "New Category";
            SetCategoryIcon(categoryName);
        }


        private Texture2D SetCategoryIcon(string category)
        {
            GUIContent iconContent;

            if (category.Contains("folder", StringComparison.OrdinalIgnoreCase))
            {
                iconContent = EditorGUIUtility.IconContent("Folder Icon");
            }
            else if (category.Contains("scene", StringComparison.OrdinalIgnoreCase))
            {
                iconContent = EditorGUIUtility.IconContent("Folder Icon");
            }
            else if (category.Contains("script", StringComparison.OrdinalIgnoreCase))
            {
                iconContent = EditorGUIUtility.IconContent("cs Script Icon");
            }
            else if (category.Contains("material", StringComparison.OrdinalIgnoreCase))
            {
                iconContent = EditorGUIUtility.IconContent("Material Icon");
            }
            else if (category.Contains("prefab", StringComparison.OrdinalIgnoreCase))
            {
                iconContent = EditorGUIUtility.IconContent("Prefab Icon");
            }
            else if (category.Contains("image", StringComparison.OrdinalIgnoreCase))
            {
                iconContent = EditorGUIUtility.IconContent("Image Icon");
            }
            else
            {
                iconContent = EditorGUIUtility.IconContent("Folder Icon");
            }

            // Return the Texture2D object
            return iconContent.image as Texture2D;
        }

        private Color SetCategoryColor(string category)
        {
            Color iconContent;

            if (category.Contains("folder", StringComparison.OrdinalIgnoreCase))
            {
                iconContent = new Color(0.5f, 0.5f, 0.8f);
            }
            else if (category.Contains("scene", StringComparison.OrdinalIgnoreCase))
            {
                iconContent = new Color(0.5f, 0.5f, 0.8f);
            }
            else if (category.Contains("script", StringComparison.OrdinalIgnoreCase))
            {
                iconContent = new Color(0.5f, 0.5f, 0.8f);
            }
            else if (category.Contains("material", StringComparison.OrdinalIgnoreCase))
            {
                iconContent = new Color(0.25f, 0.45f, 0.65f);
            }
            else if (category.Contains("prefab", StringComparison.OrdinalIgnoreCase))
            {
                iconContent = new Color(0.25f, 0.45f, 0.65f);
            }
            else if (category.Contains("image", StringComparison.OrdinalIgnoreCase))
            {
                iconContent = new Color(0.5f, 0.5f, 0.8f);
            }
            else
            {
                iconContent = new Color(0.5f, 0.5f, 0.8f);
            }

            return iconContent;
        }

        private Texture GetAssetPreview(Object item)
        {
            Texture preview = AssetPreview.GetAssetPreview(item);
            if (preview == null)
            {
                preview = AssetPreview.GetMiniThumbnail(item);
            }

            return preview;
        }

        private void SaveFavorites()
        {
            EditorPrefs.SetString("FavoriteCategories", string.Join(",", categories.ToArray()));
            foreach (var category in categorizedFavorites.Keys)
            {
                var favorites = string.Join(",", categorizedFavorites[category].ToArray());
                EditorPrefs.SetString("FavoriteItems_" + category, favorites);
                if (categoryIcons.TryGetValue(category, out var icon))
                {
                    EditorPrefs.SetString("CategoryIcon_" + category, icon);
                }
            }
        }

        private void LoadFavorites()
        {
            if (EditorPrefs.HasKey("FavoriteCategories"))
            {
                categories = EditorPrefs.GetString("FavoriteCategories").Split(',').ToList();
                foreach (var category in categories)
                {
                    if (EditorPrefs.HasKey("FavoriteItems_" + category))
                    {
                        var favorites = EditorPrefs.GetString("FavoriteItems_" + category);
                        categorizedFavorites[category] = favorites.Split(',').ToList();
                    }
                    else
                    {
                        categorizedFavorites[category] = new List<string>();
                    }

                    if (EditorPrefs.HasKey("CategoryIcon_" + category))
                    {
                        categoryIcons[category] = EditorPrefs.GetString("CategoryIcon_" + category);
                    }
                    else
                    {
                        SetCategoryIcon(category);
                    }
                }
            }
            else
            {
                categorizedFavorites["Default"] = new List<string>();
                categories = new List<string> { "Default" };
            }
        }
    }
}