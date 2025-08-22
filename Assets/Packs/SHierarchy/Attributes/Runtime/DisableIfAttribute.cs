using UnityEngine;

namespace Shadowprofile.Attributes.SHierarchy
{
    public class DisableIfAttribute : PropertyAttribute
    {
        public string Condition;
        public DisableIfAttribute(string condition) => Condition = condition;
    }
}