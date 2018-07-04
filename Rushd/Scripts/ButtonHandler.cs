using UnityEngine;

namespace Scripts
{
    public class ButtonHandler : MonoBehaviour
    {

        public string name;

        void OnEnable()
        {

        }

        public void SetDownState()
        {
            CrossPlatformInputManager.SetButtonDown(name);
        }


        public void SetUpState()
        {
            CrossPlatformInputManager.SetButtonUp(name);
        }


        public void SetAxisPositiveState()
        {
            CrossPlatformInputManager.SetAxisPositive(name);
        }


        public void SetAxisNeutralState()
        {
            CrossPlatformInputManager.SetAxisZero(name);
        }


        public void SetAxisNegativeState()
        {
            CrossPlatformInputManager.SetAxisNegative(name);
        }

        public void Update()
        {

        }
    }
}
