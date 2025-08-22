using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;
using Object = UnityEngine.Object;


#if UNITY_URP
using UnityEngine.Rendering.Universal;
#elif UNITY_HDRP
using UnityEngine.Rendering.HighDefinition;
#endif
using Shadowprofile.Utils.SHierarchy;

namespace Shadowprofile.SHierarchy
{
    [InitializeOnLoad]
    public static class CustomHierarchyEditor
    {
        public static readonly Dictionary<string, string> s_IconMap = new()
        {
            // Mesh
            { "MeshFilter", "d_MeshFilter Icon" },
            { "MeshRenderer", "d_MeshRenderer Icon" },
            { "SkinnedMeshRenderer", "SkinnedMeshRenderer Icon" },
            { "TextMeshPro", "TextMesh Icon" },

            // Tilemap
            { "Tilemap", "Tilemap Icon" },
            { "TilemapCollider2D", "TilemapCollider2D Icon" },
            { "TilemapRenderer", "TilemapRenderer Icon" },

            // Video
            { "VideoPlayer", "VideoPlayer Icon" },

            // VisualScripting
            { "ScriptMachine", "AnimatorStateMachine Icon" },
            { "StateMachine", "cs Script Icon" },
            { "Variables", "cs Script Icon" },
            { "AnimatorMessageListener", "cs Script Icon" },

            // Layout
            { "CanvasScaler", "d_CanvasScaler Icon" },
            { "Canvas", "d_Canvas Icon" },
            { "CanvasGroup", "CanvasGroup Icon" },
            { "HorizontalLayoutGroup", "HorizontalLayoutGroup Icon" },
            { "VerticalLayoutGroup", "VerticalLayoutGroup Icon" },
            { "GridLayoutGroup", "GridLayoutGroup Icon" },
            { "ContentSizeFitter", "ContentSizeFitter Icon" },
            { "LayoutElement", "LayoutElement Icon" },
            { "RectTransform", "RectTransform Icon" },
            { "AspectRatioFitter", "AspectRatioFitter Icon" },

            // UI
            { "Image", "d_Image Icon" },
            { "RawImage", "RawImage Icon" },
            { "Outline", "Outline Icon" },
            { "Button", "Button Icon" },
            { "TextMeshProUGUI", "TextMesh Icon" },
            { "TMP_Dropdown", "Dropdown Icon" },
            { "Dropdown", "Dropdown Icon" },
            { "Text", "Text Icon" },
            { "Shadow", "Shadow Icon" },
            { "Mask", "Mask Icon" },
            { "RectMask2D", "RectMask2D Icon" },
            { "ScrollRect", "ScrollRect Icon" },
            { "ScrollBar", "ScrollBar Icon" },
            { "Selectable", "Selectable Icon" },
            { "Slider", "Slider Icon" },
            { "Toggle", "Toggle Icon" },
            { "ToggleGroup", "ToggleGroup Icon" },
            { "TMP_InputField", "InputField Icon" },
            { "InputField", "InputField Icon" },
            { "PositionAsUV1", "PositionAsUV1 Icon" },

            // Rendering
            { "Camera", "d_Camera Icon" },
            { "CanvasRenderer", "d_CanvasRenderer Icon" },
            { "ReflectionProbe", "ReflectionProbe Icon" },
            { "LightProbeGroup", "LightProbeGroup Icon" },
            { "FlareLayer", "FlareLayer Icon" },
            { "LODGroup", "LODGroup Icon" },
            { "LightAnchor", "light Icon" },
            { "LightProbeProxyVolume", "LightProbeProxyVolume Icon" },
            { "OcclusionArea", "OcclusionArea Icon" },
            { "OcclusionPortal", "OcclusionPortal Icon" },
            { "Skybox", "Skybox Icon" },
            { "SortingGroup", "SortingGroup Icon" },
            { "SpriteRenderer", "SpriteRenderer Icon" },
            { "StreamingController", "StreamingController Icon" },

            // Event System
            { "EventSystem", "EventSystem Icon" },
            { "EventTrigger", "EventTrigger Icon" },
            { "GraphicRaycaster", "d_GraphicRaycaster Icon" },
            { "Physics2DRaycaster", "Physics2DRaycaster Icon" },
            { "PhysicsRaycaster", "PhysicsRaycaster Icon" },
            { "StandaloneInputModule", "StandaloneInputModule Icon" },
            { "TouchInputModule", "TouchInputModule Icon" },

            // Physics
            { "Rigidbody", "d_Rigidbody Icon" },
            { "BoxCollider", "d_BoxCollider Icon" },
            { "MeshCollider", "d_MeshCollider Icon" },
            { "CapsuleCollider", "d_CapsuleCollider Icon" },
            { "CharacterController", "CharacterController Icon" },
            { "CharacterJoint", "CharacterJoint Icon" },
            { "Cloth", "Cloth Icon" },
            { "ConfigurableJoint", "ConfigurableJoint Icon" },
            { "ConstantForce", "ConstantForce Icon" },
            { "FixedJoint", "FixedJoint Icon" },
            { "HingeJoint", "HingeJoint Icon" },
            { "SphereCollider", "SphereCollider Icon" },
            { "SpringJoint", "SpringJoint Icon" },
            { "WheelCollider", "WheelCollider Icon" },
            { "TerrainCollider", "TerrainCollider Icon" },
            { "ArticulationBody", "ArticulationBody Icon" },

            // Physics2d
            { "AreaEffector2D", "AreaEffector2D Icon" },
            { "BoxCollider2D", "d_BoxCollider2D Icon" },
            { "BuoyancyEffector2D", "BuoyancyEffector2D Icon" },
            { "CircleCollider2D", "CircleCollider2D Icon" },
            { "CapsuleCollider2D", "d_CapsuleCollider2D Icon" },
            { "CompositeCollider2D", "CompositeCollider2D Icon" },
            { "ConstantForce2D", "ConstantForce2D Icon" },
            { "CustomCollider2D", "CustomCollider2D Icon" },
            { "DistanceJoint2D", "DistanceJoint2D Icon" },
            { "EdgeCollider2D", "EdgeCollider2D Icon" },
            { "FixedJoint2D", "FixedJoint2D Icon" },
            { "FrictionJoint2D", "FrictionJoint2D Icon" },
            { "HingeJoint2D", "HingeJoint2D Icon" },
            { "PlatformEffector2D", "PlatformEffector2D Icon" },
            { "PointEffector2D", "PointEffector2D Icon" },
            { "PolygonCollider2D", "PolygonCollider2D Icon" },
            { "RelativeJoint2D", "RelativeJoint2D Icon" },
            { "Rigidbody2D", "Rigidbody Icon" },
            { "SliderJoint2D", "SliderJoint2D Icon" },
            { "SpringJoint2D", "SpringJoint Icon" },
            { "SurfaceEffector2D", "SurfaceEffector2D Icon" },
            { "TargetJoint2D", "TargetJoint2D Icon" },
            { "WheelJoint2D", "WheelJoint2D Icon" },

            // Audio
            { "AudioSource", "d_AudioSource Icon" },
            { "AudioListener", "AudioListener Icon" },
            { "AudioChorusFilter", "AudioChorusFilter Icon" },
            { "AudioDistortionFilter", "AudioDistortionFilter Icon" },
            { "AudioEchoFilter", "AudioEchoFilter Icon" },
            { "AudioHighPassFilter", "AudioHighPassFilter Icon" },
            { "AudioLowPassFilter", "AudioLowPassFilter Icon" },
            { "AudioReverbFilter", "AudioReverbFilter Icon" },
            { "AudioReverbZone", "AudioReverbZone Icon" },

            // Effets
            { "ParticleSystem", "d_ParticleSystem Icon" },
            { "Halo", "Halo Icon" },
            { "LensFlare", "LensFlare Icon" },
            { "LensFlareComponentSRP", "Flare Icon" },
            { "LineRenderer", "LineRenderer Icon" },
            { "Projector", "Projector Icon" },
            { "TrailRenderer", "TrailRenderer Icon" },
            { "VisualEffect", "VisualEffect Icon" },
            { "VFXRenderer", "d_Profiler.Rendering" },
            { "ParticleSystemRenderer", "d_Profiler.Rendering" },

            // Misc
            { "Animator", "d_Animator Icon" },
            { "Animation", "Animation Icon" },
            { "AimConstraint", "AimConstraint Icon" },
            { "BillboardRenderer", "BillboardRenderer Icon" },
            { "Grid", "Grid Icon" },
            { "LookAtConstraint", "LookAtConstraint Icon" },
            { "ParentConstraint", "ParentConstraint Icon" },
            { "RotationConstraint", "RotationConstraint Icon" },
            { "PositionConstraint", "PositionConstraint Icon" },
            { "ParticleSystemForceField", "ParticleSystemForceField Icon" },
            { "ScaleConstraint", "ScaleConstraint Icon" },
            { "SpriteMask", "SpriteMask Icon" },
            { "SpriteShapeRenderer", "SpriteShapeRenderer Icon" },
            { "Volume", "GameObject Icon" },
            { "WindZone", "WindZone Icon" },
            { "Terrain", "d_Terrain Icon" },
            { "GameManager", "GameManager Icon" },

            // Navigation
            { "NavMeshAgent", "d_NavMeshAgent Icon" },
            { "NavMeshObstacle", "NavMeshObstacle Icon" },
            { "NavMeshLink", "NavMeshLink Icon" },
            { "NavMeshModifier", "cs Script Icon" }, // TODO: Missing Icons for NavMeshModifier 
            { "NavMeshModifierVolume", "cs Script Icon" }, // TODO: Missing Icons for NavMeshModifierVolume
            { "NavMeshSurface", "cs Script Icon" }, // TODO: Missing Icons for NavMeshSurface
            { "OffMeshLink", "OffMeshLink Icon" },

            // Playables
            { "PlayableDirector", "PlayableDirector Icon" },
        };

        private const string c_URPDefine = "UNITY_URP";
        private const string c_HDRPDefine = "UNITY_HDRP";

        private static MessageBoxWindow m_MessageBoxWindow;

        private static CustomIconData s_CustomIconData
        {
            get
            {
                if (i_CustomIconData == null)
                    i_CustomIconData =
                        ShadowProfileUtils.GetAssetFromRelativePath<CustomIconData>("SHierarchy/Editor",
                            "CustomIconData");
                return i_CustomIconData;
            }
        }

        private static CustomIconData i_CustomIconData;

        private static Texture2D s_GradientTexture
        {
            get
            {
                if (i_GradientTexture == null) i_GradientTexture = CreateGradientTexture();
                return i_GradientTexture;
            }
        }

        private static Texture2D i_GradientTexture;

        private static Texture2D s_GrayTexture
        {
            get
            {
                if (i_GrayTexture == null) i_GrayTexture = CreateGrayTexture();
                return i_GrayTexture;
            }
        }

        private static Texture2D i_GrayTexture;

        private static Texture2D s_WarningIcon
        {
            get
            {
                if (i_WarningIcon == null)
                    i_WarningIcon = EditorGUIUtility.IconContent("console.warnicon").image as Texture2D;
                return i_WarningIcon;
            }
        }

        private static Texture2D i_WarningIcon;

        private static Texture2D s_ErrorIcon
        {
            get
            {
                if (i_ErrorIcon == null)
                    i_ErrorIcon = EditorGUIUtility.IconContent("console.erroricon").image as Texture2D;
                return i_ErrorIcon;
            }
        }

        private static Texture2D i_ErrorIcon;


        private static Texture2D i_pinIcon;

        private static Texture2D s_pinIcon
        {
            get
            {
                if (i_pinIcon == null)
                    i_pinIcon =
                        (Texture2D)ShadowProfileUtils.GetAssetFromRelativePath<Texture2D>("SHierarchy/Editor/Ico",
                            "pin");
                return i_pinIcon;
            }
        }

        private static GUIStyle pinButtonStyle;
        private static Color normalColor = new Color(.5f, .5f, .5f, 1f);
        private static Color hoverColor = Color.white;

        private static bool m_showIcons = true;
        private static bool m_showTittle = true;
        private static bool m_showPin = true;
        private static bool m_showWarnError = true;
     


        static CustomHierarchyEditor()
        {
            CheckRenderPipeline();
            SetSymbol();
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyWindowItemOnGUI;
      
        }

  
        
        
        private static void SetSymbol()
        {
#if !SP
            var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(buildTargetGroup);
            PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, "SP");
#endif
        }

        private static void CheckRenderPipeline()
        {
            if (GraphicsSettings.defaultRenderPipeline == null) return;

            var currentPipeline = GraphicsSettings.defaultRenderPipeline.GetType().Name;
            var buildTargetGroup = EditorUserBuildSettings.selectedBuildTargetGroup;
            var namedBuildTarget = NamedBuildTarget.FromBuildTargetGroup(buildTargetGroup);

            var defines = PlayerSettings.GetScriptingDefineSymbols(namedBuildTarget);

            switch (currentPipeline)
            {
                case "UniversalRenderPipelineAsset":
                {
                    defines = defines.Replace(c_HDRPDefine, c_URPDefine);
                    if (defines.Contains(c_URPDefine)) break;
                    defines += ";" + c_URPDefine;
                    break;
                }
                case "HDRenderPipelineAsset":
                {
                    defines = defines.Replace(c_URPDefine, c_HDRPDefine);
                    if (defines.Contains(c_HDRPDefine)) break;
                    defines += ";" + c_HDRPDefine;
                    break;
                }
            }

            PlayerSettings.SetScriptingDefineSymbols(namedBuildTarget, defines);
        }

        private static Texture2D CreateGrayTexture()
        {
            const int width = 50;
            const int height = 16;

            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            var grayColor = new Color(0.22f, 0.22f, 0.22f, 1f); // Dark gray

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    texture.SetPixel(x, y, grayColor);
                }
            }

            texture.Apply();
            return texture;
        }

        private static Texture2D CreateGradientTexture()
        {
            const int width = 50;
            const int height = 16;

            var texture = new Texture2D(width, height, TextureFormat.RGBA32, false);
            var startColor = new Color(0.1f, 0.1f, 0.1f, 1f);
            var endColor = new Color(0.1f, 0.1f, 0.1f, 0f);

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var t = (float)x / width;
                    var alpha = Mathf.Lerp(startColor.a, endColor.a, Mathf.Abs(0.52f - t) * 2f);
                    var color = new Color(startColor.r, startColor.g, startColor.b, alpha);
                    texture.SetPixel(x, y, color);
                }
            }

            texture.Apply();
            return texture;
        }

        private static void OnHierarchyWindowItemOnGUI(int instanceID, Rect selectionRect)
        {
            GetData();
            var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            if (gameObject == null) return;

            if (gameObject.name.StartsWith("==")) DrawTitleHeader(gameObject, ref selectionRect);
            else
            {
                if (m_showIcons)
                    DrawComponentsWithIcons(gameObject, selectionRect);
                if (m_showWarnError)
                    DrawWarningsAndErrors(gameObject, selectionRect);
                if (m_showPin)
                    DrawPinSystem(gameObject, selectionRect);
            }
        }

        private static void GetData()
        {
           
            m_showIcons = EditorPrefs.GetBool("ShowIconButton", true);
            m_showTittle = EditorPrefs.GetBool("ShowTittle", true);
            m_showPin = EditorPrefs.GetBool("ShowPin", true);
            m_showWarnError = EditorPrefs.GetBool("ShowWarnError", true);
        
            
        }
        private static void DrawPinSystem(GameObject gameObject, Rect selectionRect)
        {
            if (pinButtonStyle == null)
            {
                pinButtonStyle = new GUIStyle()
                {
                    normal = { background = null },
                    hover = { background = null },
                    active = { background = null },
                    focused = { background = null },
                    onNormal = { background = null },
                    onHover = { background = null },
                    onActive = { background = null },
                    onFocused = { background = null },

                    border = new RectOffset(0, 0, 0, 0),
                    padding = new RectOffset(0, 0, 0, 0),
                    margin = new RectOffset(0, 0, 0, 0),
                    overflow = new RectOffset(0, 0, 0, 0),
                    contentOffset = Vector2.zero,
                    alignment = TextAnchor.MiddleCenter,
                    imagePosition = ImagePosition.ImageOnly
                };
            }

            Rect pinButtonRect = new Rect(selectionRect.x - 30, selectionRect.y, 18, selectionRect.height);

            bool isHovering = pinButtonRect.Contains(Event.current.mousePosition);
            bool isPinned = PinnedObjectsWindow.IsPinned(gameObject);
            Color iconColor = isPinned ? new Color(.5f, .85f, 1f, 1f) : (isHovering ? hoverColor : normalColor);

            GUI.color = iconColor;

            if (GUI.Button(pinButtonRect, s_pinIcon, pinButtonStyle))
            {
                if (Event.current.control)
                {
                    PinnedObjectsWindow.UnpinObject(gameObject);
                }
                else
                {
                    PinnedObjectsWindow.PinObject(gameObject);
                }
            }

            GUI.color = Color.white;
        }

        private static void DrawTitleHeader(GameObject gameObject, ref Rect selectionRect)
        {
            if (!m_showTittle) return;
            var title = gameObject.name.Substring(2).Trim();

            var headerRect = new Rect(selectionRect.x, selectionRect.y, selectionRect.width, 16);
            GUI.DrawTexture(headerRect, s_GrayTexture);
            GUI.DrawTexture(headerRect, s_GradientTexture);

            var style = new GUIStyle(EditorStyles.label)
            {
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState { textColor = Color.white },
                alignment = TextAnchor.MiddleCenter,
                fontSize = 12
            };
            GUI.Label(headerRect, title, style);

            selectionRect.y += 16;
            selectionRect.height -= 16;
        }

//OLD VERSION
//         private static void DrawComponentsWithIcons(GameObject gameObject, Rect selectionRect)
//         {
//             var componentRect = new Rect(selectionRect.x + selectionRect.width - 15, selectionRect.y, 15, 15);
//
//             var components = gameObject.GetComponents<Component>()
//                 .Where(c => c != null && c is not Transform
// #if UNITY_URP
//                 and not UniversalAdditionalLightData
//                 and not UniversalAdditionalCameraData
// #endif
// #if UNITY_HDRP
//                         and not HDAdditionalLightData
//                         and not HDAdditionalCameraData
// #endif
//                     and not ParticleSystemRenderer
//                 )
//                 .ToArray();
//
//             var groupedComponents = components
//                 .GroupBy(c => c.GetType())
//                 .ToDictionary(g => g.Key, g => g.ToList());
//
//             foreach (var group in groupedComponents.OrderByDescending(g => g.Key.Name))
//             {
//                 var icon = GetComponentIcon(group.Key, group.Value);
//
//                 if (icon == null)
//                 {
//                     Debug.LogWarning($"Icon for {group.Key.Name} is null.");
//                     continue;
//                 }
//
//                 if (GUI.Button(componentRect, new GUIContent(icon), GUIStyle.none))
//                 {
//                     OpenInspector(group.Value.ToArray());
//                 }
//
//                 componentRect.x -= 15;
//             }
//         }
        private static void DrawComponentsWithIcons(GameObject gameObject, Rect selectionRect)
        {
            var componentRect = new Rect(selectionRect.x + selectionRect.width - 15, selectionRect.y, 15, 15);
            int maxIcons = EditorPrefs.GetInt("MaxHierarchyIcons", 10);
            var components = gameObject.GetComponents<Component>()
                .Where(c => c != null && c is not Transform
#if UNITY_URP
        and not UniversalAdditionalLightData
        and not UniversalAdditionalCameraData
#endif
#if UNITY_HDRP
                    and not HDAdditionalLightData
                    and not HDAdditionalCameraData
#endif
                    and not ParticleSystemRenderer)
                .ToArray();

            var groupedComponents = components
                .GroupBy(c => c.GetType())
                .ToDictionary(g => g.Key, g => g.ToList());

            if (groupedComponents.Count > maxIcons)
            {
                var moreIcon = EditorGUIUtility.IconContent("d_Folder Icon").image as Texture2D;
                if (GUI.Button(componentRect, new GUIContent(moreIcon, "Show All Components"), GUIStyle.none))
                {
                    ComponentListPopupWindow.Show(gameObject, groupedComponents);
                }

                return;
            }

            foreach (var group in groupedComponents.OrderByDescending(g => g.Key.Name))
            {
                var icon = GetComponentIcon(group.Key, group.Value);

                if (icon == null)
                {
                    //   Debug.LogWarning($"Icon for {group.Key.Name} is null.");
                    continue;
                }

                if (GUI.Button(componentRect, new GUIContent(icon), GUIStyle.none))
                {
                    OpenInspector(group.Value.ToArray());
                }

                componentRect.x -= 15;
            }
        }

        public static Texture2D GetComponentIcon(Type componentType, List<Component> components)
        {
            var componentName = componentType.Name;

            var customIcon = GetCustomIcon(componentName);
            if (customIcon != null) return customIcon;

            if (componentType == typeof(Light))
            {
                var light = components.FirstOrDefault() as Light;
                if (light != null)
                {
                    return EditorGUIUtility.IconContent(light.type switch
                    {
                        LightType.Directional => "DirectionalLight Icon",
                        LightType.Point => "Light Icon",
                        LightType.Spot => "SpotLight Icon",
                        LightType.Rectangle => "AreaLight Icon",
                        _ => "d_Light Icon"
                    }).image as Texture2D;
                }
            }

            var iconName = componentType.Name;
            if (!s_IconMap.TryGetValue(iconName, out var iconContentName))
                return EditorGUIUtility.IconContent("cs Script Icon").image as Texture2D;

            if (iconContentName == string.Empty) return GetCustomIcon(iconName);

            return EditorGUIUtility.IconContent(iconContentName)?.image as Texture2D;
        }

        private static Texture2D GetCustomIcon(string componentName)
        {
            if (s_CustomIconData == null) return null;
            var iconPaths = s_CustomIconData.GetIconPaths();
            return iconPaths.TryGetValue(componentName, out var iconPath)
                ? AssetDatabase.LoadAssetAtPath<Texture2D>(iconPath)
                : null;
        }

        public static void OpenInspector(Component[] components)
        {
            if (components == null || components.Length == 0) return;

            var window = EditorWindow.GetWindow<FloatingInspectorWindow>(false, null, false);
            window.titleContent =
                new GUIContent("Quick Inspector", EditorGUIUtility.IconContent("d_Settings Icon").image);
            window.SetTargets(components);
            window.Show();
        }

        private static void DrawWarningsAndErrors(GameObject gameObject, Rect selectionRect)
        {
            var (warnings, errors) = CheckForWarningsAndErrors(gameObject);

            if (warnings.Count <= 0 && errors.Count <= 0) return;

            var iconRect = new Rect
            {
                x = selectionRect.x + (selectionRect.width - 16) / 2,
                y = selectionRect.y + (selectionRect.height - 16) / 2,
                width = 16,
                height = 16
            };

            var icon = errors.Count > 0 ? s_ErrorIcon : s_WarningIcon;
            GUI.DrawTexture(iconRect, icon);

            if (!GUI.Button(iconRect, new GUIContent(icon), GUIStyle.none)) return;
            MessageBoxWindow.ShowWindow(warnings, errors);
        }

        private static (List<string> warnings, List<string> errors) CheckForWarningsAndErrors(GameObject gameObject)
        {
            var warnings = new List<string>();
            var errors = new List<string>();
            var hasEventSystem = gameObject.GetComponent<EventSystem>() != null;
            var hasAudioListener = gameObject.GetComponent<AudioListener>() != null;
            var components = gameObject.GetComponents<Component>();

            if (HasColliderNegative(gameObject))
            {
                warnings.Add($"Collider on {gameObject.name} has negative size or radius.");
            }

            if (HasMissingScripts(components))
            {
                errors.Add($"Missing scripts on {gameObject.name}.");
            }

            var LODMeshIssues = HasLODMeshIssues(gameObject);
            switch (LODMeshIssues)
            {
                case 1:
                    errors.Add($"LODGroup in {gameObject.name} has missing renderers.");
                    break;
                case 2:
                    errors.Add($"LODGroup in {gameObject.name} has missing meshes.");
                    break;
            }

            if (HasMissingReferences(components))
            {
                warnings.Add($"GameObject {gameObject.name} contains missing references in its components.");
            }

            if (hasEventSystem && HasMultipleEventSystems())
            {
                warnings.Add(
                    "Multiple EventSystems detected in the scene.\n Remove unnecessary Event Systems to avoid problems.");
            }

            if (hasAudioListener && HasMultipleAudioListeners())
            {
                warnings.Add(
                    "Multiple AudioListeners detected in the scene.\n Remove unnecessary Audio Listeners to avoid problems.");
            }

            return (warnings, errors);
        }

        private static bool HasColliderNegative(GameObject gameObject)
        {
            foreach (var collider in gameObject.GetComponents<Collider>())
            {
                switch (collider)
                {
                    case BoxCollider boxCollider
                        when boxCollider.size.x <= 0 || boxCollider.size.y <= 0 || boxCollider.size.z <= 0:
                    case SphereCollider sphereCollider when sphereCollider.radius <= 0:
                    case CapsuleCollider capsuleCollider
                        when (capsuleCollider.radius <= 0 || capsuleCollider.height <= 0):
                        return true;
                }
            }

            return false;
        }

        private static bool HasMissingScripts(Component[] components)
        {
            foreach (var component in components)
            {
                if (component == null) return true;
            }

            return false;
        }

        private static int HasLODMeshIssues(GameObject gameObject)
        {
            var lodGroups = gameObject.GetComponentsInChildren<LODGroup>();
            foreach (var lodGroup in lodGroups)
            {
                var lods = lodGroup.GetLODs();
                foreach (var lod in lods)
                {
                    foreach (var renderer in lod.renderers)
                    {
                        if (renderer == null) return 1;
                        if (renderer is not MeshRenderer meshRenderer) continue;

                        var meshFilter = meshRenderer.GetComponent<MeshFilter>();
                        if (meshFilter != null && meshFilter.sharedMesh == null) return 2;
                    }
                }
            }

            return 0;
        }

        private static bool HasMissingReferences(Component[] components)
        {
            foreach (var component in components)
            {
                if (component == null) continue;

                var serializedObject = new SerializedObject(component);
                var property = serializedObject.GetIterator();

                while (property.NextVisible(true))
                {
                    if (property.propertyType == SerializedPropertyType.ObjectReference
                        && property.objectReferenceValue == null
                        && property.objectReferenceInstanceIDValue != 0)
                        return true;
                }
            }

            return false;
        }

        private static bool HasMultipleEventSystems() =>
            Object.FindObjectsByType<EventSystem>(FindObjectsSortMode.None).Length > 1;

        private static bool HasMultipleAudioListeners() =>
            Object.FindObjectsByType<AudioListener>(FindObjectsSortMode.None).Length > 1;
    }
}