using JetBrains.Annotations;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

// ReSharper disable once CheckNamespace
namespace UnityStandardAssets.Cameras
{
    public class FreeLookCam : PivotBasedCameraRig
    {
        // This script is designed to be placed on the root object of a camera rig,
        // comprising 3 gameobjects, each parented to the next:

        // 	Camera Rig (Штатив)
        // 		Pivot (Точка опоры)
        // 			Camera (Камера)

        /// <summary>
        /// Как быстро двигается штатив, чтобы не отставать от цели.
        /// </summary>
        [SerializeField] private float moveSpeed = 1f;

        /// <summary>
        /// Как быстро вращается штатив, при вводе пользователя.
        /// </summary>
        [Range(0f, 10f)] [SerializeField] private float turnSpeed = 1.5f;

        /// <summary>
        /// Степень сглаживания, при повороте штатива с помощью мыши.
        /// </summary>
        [SerializeField] private float turnSmoothing = 0.0f;

        /// <summary>
        /// Максимальное значения оси x для поворота точки опоры.
        /// </summary>
        [SerializeField] private float tiltMax = 75f;

        /// <summary>
        /// Минимальное значение оси x для поворота точки опоры.
        /// </summary>
        [SerializeField] private float tiltMin = 45f;

        /// <summary>
        /// Должен ли курсор захватываться.
        /// </summary>
        [SerializeField] private bool lockCursor = false;

        /// <summary>
        /// Должна ли вертикальная ось возврощатся в исходное положение.
        /// </summary>
        [SerializeField] private bool verticalAutoReturn = false;

        /// <summary>
        /// Вращение штатива по оси Y.
        /// </summary>
        private float lookAngle;

        /// <summary>
        /// Вращение штатива по оси X.
        /// </summary>
        private float tiltAngle;

        /// <summary>
        /// How far in front of the pivot the character's look target is.
        /// </summary>
        //private const float LookDistance = 100f;

        private Vector3 pivotEulers;

		private Quaternion pivotTargetRot;

		private Quaternion transformTargetRot;

        protected override void Awake()
        {
            base.Awake();
            // Lock or unlock the cursor.
            Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !lockCursor;
			pivotEulers = pivot.rotation.eulerAngles;

	        pivotTargetRot = pivot.transform.localRotation;
			transformTargetRot = transform.localRotation;
        }


        [UsedImplicitly]
        protected void Update()
        {
            HandleRotationMovement();
            if (lockCursor && Input.GetMouseButtonUp(0))
            {
                Cursor.lockState = lockCursor ? CursorLockMode.Locked : CursorLockMode.None;
                Cursor.visible = !lockCursor;
            }
        }


        [UsedImplicitly]
        private void OnDisable()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }


        protected override void FollowTarget(float deltaTime)
        {
            if (target == null) return;
            // Move the rig towards target position.
            transform.position = Vector3.Lerp(transform.position, target.position, deltaTime*moveSpeed);
        }


        private void HandleRotationMovement()
        {
			if(Time.timeScale < float.Epsilon)
			return;

            // Read the user input
            var x = CrossPlatformInputManager.GetAxis("Mouse X");
            var y = CrossPlatformInputManager.GetAxis("Mouse Y");

            // Adjust the look angle by an amount proportional to the turn speed and horizontal input.
            lookAngle += x*turnSpeed;

            // Rotate the rig (the root object) around Y axis only:
            transformTargetRot = Quaternion.Euler(0f, lookAngle, 0f);

            if (verticalAutoReturn)
            {
                // For tilt input, we need to behave differently depending on whether we're using mouse or touch input:
                // on mobile, vertical input is directly mapped to tilt value, so it springs back automatically when the look input is released
                // we have to test whether above or below zero because we want to auto-return to zero even if min and max are not symmetrical.
                tiltAngle = y > 0 ? Mathf.Lerp(0, -tiltMin, y) : Mathf.Lerp(0, tiltMax, -y);
            }
            else
            {
                // on platforms with a mouse, we adjust the current angle based on Y mouse input and turn speed
                tiltAngle -= y*turnSpeed;
                // and make sure the new value is within the tilt range
                tiltAngle = Mathf.Clamp(tiltAngle, -tiltMin, tiltMax);
            }

            // Tilt input around X is applied to the pivot (the child of this object)
			pivotTargetRot = Quaternion.Euler(tiltAngle, pivotEulers.y , pivotEulers.z);

			if (turnSmoothing > 0)
			{
				pivot.localRotation = Quaternion.Slerp(pivot.localRotation, pivotTargetRot, turnSmoothing * Time.deltaTime);
				transform.localRotation = Quaternion.Slerp(transform.localRotation, transformTargetRot, turnSmoothing * Time.deltaTime);
			}
			else
			{
				pivot.localRotation = pivotTargetRot;
				transform.localRotation = transformTargetRot;
			}
        }
    }
}
