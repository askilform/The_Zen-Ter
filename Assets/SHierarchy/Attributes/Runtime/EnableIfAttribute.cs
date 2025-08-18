using UnityEngine;

namespace Shadowprofile.Attributes.SHierarchy
{
    public class EnableIfAttribute : PropertyAttribute
    {
        public string Condition;
        public EnableIfAttribute(string condition) => Condition = condition;
    }
}