using System;
using System.Collections.Generic;
using UnityEngine;
namespace Shadowprofile.SHierarchy
{
    [Serializable]
    public class IconEntry
    {
        public string componentName;
        public string iconPath;
    }

    [Serializable]
    public class CustomIconData : ScriptableObject
    {
        [SerializeField]
        private List<IconEntry> iconEntries = new();

        public Dictionary<string, string> GetIconPaths()
        {
            var iconPaths = new Dictionary<string, string>();
            foreach (var entry in iconEntries)
            {
                iconPaths[entry.componentName] = entry.iconPath;
            }
            return iconPaths;
        }

        public void SetIconPath(string componentName, string iconPath)
        {
            var entry = iconEntries.Find(e => e.componentName == componentName);

            if (entry == null)
            {
                iconEntries.Add(new IconEntry { componentName = componentName, iconPath = iconPath });
                return;
            }

            entry.iconPath = iconPath;
        }

        public void RemoveIconPath(string componentName) => iconEntries.RemoveAll(e => e.componentName == componentName);

        public void ClearIcons() => iconEntries.Clear();
    }
}