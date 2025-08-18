using System;
using UnityEngine;

namespace Shadowprofile.Attributes.SHierarchy
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class HideIfAttribute : PropertyAttribute
    {
        public string ConditionProperty { get; private set; }

        public HideIfAttribute(string conditionProperty)
        {
            ConditionProperty = conditionProperty;
        }
    }
}