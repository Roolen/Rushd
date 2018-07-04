using UnityEngine;
using UnityEngine.EventSystems;

namespace Scripts
{
	public class Joystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler
	{
		public enum AxisOption
		{
			// Options for which axes to use
			Both, // Use both
			OnlyHorizontal, // Only horizontal
			OnlyVertical // Only vertical
		}

		public int movementRange = 100;
		public AxisOption axesToUse = AxisOption.Both; // The options for the axes that the still will use
		public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
		public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input

		Vector3 mStartPos;
		bool mUseX; // Toggle for using the x axis
		bool mUseY; // Toggle for using the Y axis
		CrossPlatformInputManager.VirtualAxis mHorizontalVirtualAxis; // Reference to the joystick in the cross platform input
		CrossPlatformInputManager.VirtualAxis mVerticalVirtualAxis; // Reference to the joystick in the cross platform input

		void OnEnable()
		{
			CreateVirtualAxes();
		}

        void Start()
        {
            mStartPos = transform.position;
        }

		void UpdateVirtualAxes(Vector3 value)
		{
			var delta = mStartPos - value;
			delta.y = -delta.y;
			delta /= movementRange;
			if (mUseX)
			{
				mHorizontalVirtualAxis.Update(-delta.x);
			}

			if (mUseY)
			{
				mVerticalVirtualAxis.Update(delta.y);
			}
		}

		void CreateVirtualAxes()
		{
			// set axes to use
			mUseX = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyHorizontal);
			mUseY = (axesToUse == AxisOption.Both || axesToUse == AxisOption.OnlyVertical);

			// create new axes based on axes to use
			if (mUseX)
			{
				mHorizontalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(horizontalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(mHorizontalVirtualAxis);
			}
			if (mUseY)
			{
				mVerticalVirtualAxis = new CrossPlatformInputManager.VirtualAxis(verticalAxisName);
				CrossPlatformInputManager.RegisterVirtualAxis(mVerticalVirtualAxis);
			}
		}


		public void OnDrag(PointerEventData data)
		{
			Vector3 newPos = Vector3.zero;

			if (mUseX)
			{
				int delta = (int)(data.position.x - mStartPos.x);
				delta = Mathf.Clamp(delta, - movementRange, movementRange);
				newPos.x = delta;
			}

			if (mUseY)
			{
				int delta = (int)(data.position.y - mStartPos.y);
				delta = Mathf.Clamp(delta, -movementRange, movementRange);
				newPos.y = delta;
			}
			transform.position = new Vector3(mStartPos.x + newPos.x, mStartPos.y + newPos.y, mStartPos.z + newPos.z);
			UpdateVirtualAxes(transform.position);
		}


		public void OnPointerUp(PointerEventData data)
		{
			transform.position = mStartPos;
			UpdateVirtualAxes(mStartPos);
		}


		public void OnPointerDown(PointerEventData data) { }

		void OnDisable()
		{
			// remove the joysticks from the cross platform input
			if (mUseX)
			{
				mHorizontalVirtualAxis.Remove();
			}
			if (mUseY)
			{
				mVerticalVirtualAxis.Remove();
			}
		}
	}
}