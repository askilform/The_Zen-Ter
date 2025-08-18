using UnityEngine;

/// <summary>
/// Custom attribute to display information as a label above a field in the Unity Inspector.
/// </summary>
namespace Shadowprofile.Attributes.SHierarchy
{
    public class LabelAttribute : PropertyAttribute
    {
        public string InfoText { get; private set; }


        public LabelAttribute(string infoText)
        {
            InfoText = $" \n{infoText}\n ";
        }
    }
}