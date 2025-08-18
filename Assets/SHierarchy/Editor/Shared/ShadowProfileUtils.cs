using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace Shadowprofile.Utils.SHierarchy
{
    public static class ShadowProfileUtils
    {
        public static T GetAssetFromRelativePath<T>(string relativePath, string name = "", string filter = "") where T : Object
        {
            var foundAssets = AssetDatabase.FindAssets($"t:{typeof(T).Name} {name} {filter}");
            T asset = null;

            for (var i = 0; i < foundAssets.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(foundAssets[i]);
                if (path.Contains(relativePath))
                {
                    asset = AssetDatabase.LoadAssetAtPath<T>(path);
                    break;
                }
            }

            if (asset == null)
            {
                //  Debug.LogError($"{typeof(T)} '{name}' not found.");
                return null;
            }
            return asset;
        }

        public static T[] GetAssetsFromRelativePath<T>(string relativePath) where T : Object
        {
            var foundAssets = AssetDatabase.FindAssets($"t:{typeof(T).Name}");
            var assets = new List<T>();

            for (var i = 0; i < foundAssets.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(foundAssets[i]);
                if (path.Contains(relativePath))
                {
                    assets.Add(AssetDatabase.LoadAssetAtPath<T>(path));
                }
            }

            return assets.ToArray();
        }

        public static string GetFullPathFromRelativePath(string relativePath, string filter = "")
        {
            var asset = GetAssetFromRelativePath<Object>(relativePath, filter: filter);
            if (asset == null) return string.Empty;
            var path = AssetDatabase.GetAssetPath(asset);
            var index = path.IndexOf(relativePath, StringComparison.Ordinal);
            return path.Substring(0, index + relativePath.Length);
        }
    }
}