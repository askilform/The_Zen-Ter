using UnityEngine;
using Shadowprofile.Attributes.SHierarchy;

namespace ShadowProfile.SHierarchy
{
    public class TestScript : MonoBehaviour
    {

        [SerializeField] float Value01;
        [SerializeField] float Value02;
        [SerializeField] float Value03;
        [SerializeField][Range(1, 10)] float Value04;
        [Label("The Object Will do Someting!\nEnter your Text Here!")]
        [SerializeField] GameObject ObjectValue;
        [ReadOnly]
        public string text = "test readonly property";

        #region Button Attribute
        [Button]
        private void ButtonMethod()
        {
            Debug.Log("Button Clicked!");
        }

        [Button("Button With Custom Name")]
        private void ButtonMethod2()
        {
            Debug.Log("Button Clicked!");
        }
        #endregion

        [Separator(1, "Min Max Slider Attribute")]
        [MinMaxSlider(0, 2)]
        public Vector2 widthRange;

        [SerializeField] private float newValue;
        [Separator(2,"If Enabled / Disabled Attribute")]
        public bool IsEnabled = false;
        [EnableIf("IsEnabled")]
        public int enabledvalue = 0;

        [DisableIf("IsEnabled")]
        public int disableValue = 0;
        [Separator(separatorTitle = "Hide / Show If Attribute")]
        
        public bool isHiding = false;
        [HideIf("isHiding")]
        public int enableInt = 0;
        [HideIf("isHiding")]
        public int enableFloat = 0;

        [ShowIf("isHiding")]
        public int DisableInt = 0;
        [ShowIf("isHiding")]
        public int DisableFloat = 0;
        
        public int otherProp;

    }
}