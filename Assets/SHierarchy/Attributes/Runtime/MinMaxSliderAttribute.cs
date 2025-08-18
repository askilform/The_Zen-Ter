using UnityEngine;
namespace  Shadowprofile.Attributes.SHierarchy
{
public class MinMaxSliderAttribute : PropertyAttribute
{
    public float MinLimit;
    public float MaxLimit;

    public MinMaxSliderAttribute(float minLimit, float maxLimit)
    {
        MinLimit = minLimit;
        MaxLimit = maxLimit;
    }
}
}