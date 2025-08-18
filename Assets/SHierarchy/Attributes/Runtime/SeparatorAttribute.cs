using UnityEngine;

namespace Shadowprofile.Attributes.SHierarchy
{
    public class SeparatorAttribute : PropertyAttribute
    {
        public float thickness;
        public string separatorTitle;

        public SeparatorAttribute(float _thickneness = 2, string _separatorTitle = null)
        {
            this.thickness = _thickneness;
            this.separatorTitle = _separatorTitle;
        }
    }
}