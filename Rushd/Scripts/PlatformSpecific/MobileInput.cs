using UnityEngine;

namespace Scripts.PlatformSpecific
{
    public class MobileInput : VirtualInput
    {
        private void AddButton(string name)
        {
            // we have not registered this button yet so add it, happens in the constructor
            CrossPlatformInputManager.RegisterVirtualButton(new CrossPlatformInputManager.VirtualButton(name));
        }


        private void AddAxes(string name)
        {
            // we have not registered this button yet so add it, happens in the constructor
            CrossPlatformInputManager.RegisterVirtualAxis(new CrossPlatformInputManager.VirtualAxis(name));
        }


        public override float GetAxis(string name, bool raw)
        {
            if (!mVirtualAxes.ContainsKey(name))
            {
                AddAxes(name);
            }
            return mVirtualAxes[name].GetValue;
        }


        public override void SetButtonDown(string name)
        {
            if (!mVirtualButtons.ContainsKey(name))
            {
                AddButton(name);
            }
            mVirtualButtons[name].Pressed();
        }


        public override void SetButtonUp(string name)
        {
            if (!mVirtualButtons.ContainsKey(name))
            {
                AddButton(name);
            }
            mVirtualButtons[name].Released();
        }


        public override void SetAxisPositive(string name)
        {
            if (!mVirtualAxes.ContainsKey(name))
            {
                AddAxes(name);
            }
            mVirtualAxes[name].Update(1f);
        }


        public override void SetAxisNegative(string name)
        {
            if (!mVirtualAxes.ContainsKey(name))
            {
                AddAxes(name);
            }
            mVirtualAxes[name].Update(-1f);
        }


        public override void SetAxisZero(string name)
        {
            if (!mVirtualAxes.ContainsKey(name))
            {
                AddAxes(name);
            }
            mVirtualAxes[name].Update(0f);
        }


        public override void SetAxis(string name, float value)
        {
            if (!mVirtualAxes.ContainsKey(name))
            {
                AddAxes(name);
            }
            mVirtualAxes[name].Update(value);
        }


        public override bool GetButtonDown(string name)
        {
            if (mVirtualButtons.ContainsKey(name))
            {
                return mVirtualButtons[name].GetButtonDown;
            }

            AddButton(name);
            return mVirtualButtons[name].GetButtonDown;
        }


        public override bool GetButtonUp(string name)
        {
            if (mVirtualButtons.ContainsKey(name))
            {
                return mVirtualButtons[name].GetButtonUp;
            }

            AddButton(name);
            return mVirtualButtons[name].GetButtonUp;
        }


        public override bool GetButton(string name)
        {
            if (mVirtualButtons.ContainsKey(name))
            {
                return mVirtualButtons[name].GetButton;
            }

            AddButton(name);
            return mVirtualButtons[name].GetButton;
        }


        public override Vector3 MousePosition()
        {
            return VirtualMousePosition;
        }
    }
}
