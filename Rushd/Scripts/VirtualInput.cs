using System.Collections.Generic;
using UnityEngine;

namespace Scripts
{
    public abstract class VirtualInput
    {
        public Vector3 VirtualMousePosition { get; private set; }
        
        
        protected Dictionary<string, CrossPlatformInputManager.VirtualAxis> mVirtualAxes =
            new Dictionary<string, CrossPlatformInputManager.VirtualAxis>();
            // Dictionary to store the name relating to the virtual axes
        protected Dictionary<string, CrossPlatformInputManager.VirtualButton> mVirtualButtons =
            new Dictionary<string, CrossPlatformInputManager.VirtualButton>();
        protected List<string> mAlwaysUseVirtual = new List<string>();
            // list of the axis and button names that have been flagged to always use a virtual axis or button
        

        public bool AxisExists(string name)
        {
            return mVirtualAxes.ContainsKey(name);
        }

        public bool ButtonExists(string name)
        {
            return mVirtualButtons.ContainsKey(name);
        }


        public void RegisterVirtualAxis(CrossPlatformInputManager.VirtualAxis axis)
        {
            // check if we already have an axis with that name and log and error if we do
            if (mVirtualAxes.ContainsKey(axis.Name))
            {
                Debug.LogError("There is already a virtual axis named " + axis.Name + " registered.");
            }
            else
            {
                // add any new axes
                mVirtualAxes.Add(axis.Name, axis);

                // if we dont want to match with the input manager setting then revert to always using virtual
                if (!axis.MatchWithInputManager)
                {
                    mAlwaysUseVirtual.Add(axis.Name);
                }
            }
        }


        public void RegisterVirtualButton(CrossPlatformInputManager.VirtualButton button)
        {
            // check if already have a buttin with that name and log an error if we do
            if (mVirtualButtons.ContainsKey(button.Name))
            {
                Debug.LogError("There is already a virtual button named " + button.Name + " registered.");
            }
            else
            {
                // add any new buttons
                mVirtualButtons.Add(button.Name, button);

                // if we dont want to match to the input manager then always use a virtual axis
                if (!button.MatchWithInputManager)
                {
                    mAlwaysUseVirtual.Add(button.Name);
                }
            }
        }


        public void UnRegisterVirtualAxis(string name)
        {
            // if we have an axis with that name then remove it from our dictionary of registered axes
            if (mVirtualAxes.ContainsKey(name))
            {
                mVirtualAxes.Remove(name);
            }
        }


        public void UnRegisterVirtualButton(string name)
        {
            // if we have a button with this name then remove it from our dictionary of registered buttons
            if (mVirtualButtons.ContainsKey(name))
            {
                mVirtualButtons.Remove(name);
            }
        }


        // returns a reference to a named virtual axis if it exists otherwise null
        public CrossPlatformInputManager.VirtualAxis VirtualAxisReference(string name)
        {
            return mVirtualAxes.ContainsKey(name) ? mVirtualAxes[name] : null;
        }


        public void SetVirtualMousePositionX(float f)
        {
            VirtualMousePosition = new Vector3(f, VirtualMousePosition.y, VirtualMousePosition.z);
        }


        public void SetVirtualMousePositionY(float f)
        {
            VirtualMousePosition = new Vector3(VirtualMousePosition.x, f, VirtualMousePosition.z);
        }


        public void SetVirtualMousePositionZ(float f)
        {
            VirtualMousePosition = new Vector3(VirtualMousePosition.x, VirtualMousePosition.y, f);
        }


        public abstract float GetAxis(string name, bool raw);
        
        public abstract bool GetButton(string name);
        public abstract bool GetButtonDown(string name);
        public abstract bool GetButtonUp(string name);

        public abstract void SetButtonDown(string name);
        public abstract void SetButtonUp(string name);
        public abstract void SetAxisPositive(string name);
        public abstract void SetAxisNegative(string name);
        public abstract void SetAxisZero(string name);
        public abstract void SetAxis(string name, float value);
        public abstract Vector3 MousePosition();
    }
}
