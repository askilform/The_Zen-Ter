using System;
using UnityEngine;

namespace Shadowprofile.Attributes.SHierarchy
{
    [AttributeUsage(AttributeTargets.Field, Inherited = true, AllowMultiple = false)]
    public class ShowIfAttribute : PropertyAttribute
    {
        public string ConditionProperty { get; private set; }

        public ShowIfAttribute(string conditionProperty)
        {
            ConditionProperty = conditionProperty;
        }
    }
}