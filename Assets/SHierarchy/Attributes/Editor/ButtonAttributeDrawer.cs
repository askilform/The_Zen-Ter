using UnityEngine;
using UnityEditor;
using System.Reflection;

namespace  Shadowprofile.Attributes.SHierarchy
{
[CustomEditor(typeof(MonoBehaviour), true)]
public class ButtonAttributeDrawer : Editor
{
    public override void OnInspectorGUI()
    {
        // Draw the default inspector
        DrawDefaultInspector();

        // Get the target type
        var targetType = target.GetType();
        // Get all public and private methods of the target
        var methods = targetType.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

        // Loop through all methods to check for the Button attribute
        foreach (var method in methods)
        {
            // Check if the method has the Button attribute
            var attribute = method.GetCustomAttribute<ButtonAttribute>();
            if (attribute != null)
            {
                // If the attribute has a custom label, use it; otherwise, use the method name
                string buttonLabel = string.IsNullOrEmpty(attribute.buttonLabel) ? method.Name : attribute.buttonLabel;

                // Draw the button in the Inspector
                if (GUILayout.Button(buttonLabel))
                {
                    // Invoke the method when the button is clicked
                    method.Invoke(target, null);

                    // Mark the object as dirty to save changes
                    EditorUtility.SetDirty(target);
                }
            }
        }
    }
}
}