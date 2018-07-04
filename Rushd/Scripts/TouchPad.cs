using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Scripts
{
	[RequireComponent(typeof(Image))]
	public class TouchPad : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
	{
		// Options for which axes to use
		public enum AxisOption
		{
			Both, // Use both
			OnlyHorizontal, // Only horizontal
			OnlyVertical // Only vertical
		}


		public enum ControlStyle
		{
			Absolute, // operates from teh center of the image
			Relative, // operates from the center of the initial touch
			Swipe, // swipe to touch touch no maintained center
		}


		public AxisOption axesToUse = AxisOption.Both; // The options for the axes that the still will use
		public ControlStyle controlStyle = ControlStyle.Absolute; // control style to use
		public string horizontalAxisName = "Horizontal"; // The name given to the horizontal axis for the cross platform input
		public string verticalAxisName = "Vertical"; // The name given to the vertical axis for the cross platform input
		public float xsensitivity = 1f;
		public float ysensitivity = 1f;

		Vector3 mStartPos;
		Vector2 mPreviousDelta;
		Vector3 mJoytickOutput;
		bool mUseX; // Toggle for using the x axis
		bool mUseY; // Toggle for using the Y axis
		CrossPlatformInputManager.VirtualAxis mHorizontalVirtualAxis; // Reference to the joystick in the cross platform input
		CrossPlatformInputManager.VirtualAxis mVerticalVirtualAxis; // Reference to the joystick in the cross platform input
		bool mDragging;
		int mId = -1;
		Vector2 mPreviousTouchPos; // swipe style control touch


#if !UNITY_EDITOR
    private Vector3 m_Center;
    private Image m_Image;
#else
		Vector3 mPreviousMouse;
#endif

		void OnEnable()
		{
			CreateVirtualAxes();
		}

        void Start()
        {
#if !UNITY_EDITOR
            m_Image = GetComponent<Image>();
            m_Center = m_Image.transform.position;
#endif
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

		void UpdateVirtualAxes(Vector3 value)
		{
			value = value.normalized;
			if (mUseX)
			{
				mHorizontalVirtualAxis.Update(value.x);
			}

			if (mUseY)
			{
				mVerticalVirtualAxis.Update(value.y);
			}
		}


		public void OnPointerDown(PointerEventData data)
		{
			mDragging = true;
			mId = data.pointerId;
#if !UNITY_EDITOR
        if (controlStyle != ControlStyle.Absolute )
            m_Center = data.position;
#endif
		}

		void Update()
		{
			if (!mDragging)
			{
				return;
			}
			if (Input.touchCount >= mId + 1 && mId != -1)
			{
#if !UNITY_EDITOR

            if (controlStyle == ControlStyle.Swipe)
            {
                m_Center = m_PreviousTouchPos;
                m_PreviousTouchPos = Input.touches[m_Id].position;
            }
            Vector2 pointerDelta = new Vector2(Input.touches[m_Id].position.x - m_Center.x , Input.touches[m_Id].position.y - m_Center.y).normalized;
            pointerDelta.x *= Xsensitivity;
            pointerDelta.y *= Ysensitivity;
#else
				Vector2 pointerDelta;
				pointerDelta.x = Input.mousePosition.x - mPreviousMouse.x;
				pointerDelta.y = Input.mousePosition.y - mPreviousMouse.y;
				mPreviousMouse = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
#endif
				UpdateVirtualAxes(new Vector3(pointerDelta.x, pointerDelta.y, 0));
			}
		}


		public void OnPointerUp(PointerEventData data)
		{
			mDragging = false;
			mId = -1;
			UpdateVirtualAxes(Vector3.zero);
		}

		void OnDisable()
		{
			if (CrossPlatformInputManager.AxisExists(horizontalAxisName))
				CrossPlatformInputManager.UnRegisterVirtualAxis(horizontalAxisName);

			if (CrossPlatformInputManager.AxisExists(verticalAxisName))
				CrossPlatformInputManager.UnRegisterVirtualAxis(verticalAxisName);
		}
	}
}