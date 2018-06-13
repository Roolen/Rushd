using UnityEngine;

namespace Assets.Scripts
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [RequireComponent(typeof(Rigidbody))]
    public class TankController : MonoBehaviour
    {
        [Header("Скорость танка")]
        public float sX;
        public float sY;
        public float sZ;

        [Header("Скорость поворота")]
        [SerializeField]
        private float speedTurn;

        [Header("Скорость движения назад")]
        [SerializeField]
        private float speedBack;

        [Header("Скорость движения вперед")]
        [SerializeField]
        private float speedForward;

        [Header("Максимальная скорость")]
        [SerializeField]
        private float maxSpeed;

        [Header("Высота парения")]
        [SerializeField]
        private int hoverHeight;

        private Rigidbody thisRigidbody;

        #region Properties

        public float SpeedTurn
        {
            get
            {
                return speedTurn;
            }

            set
            {
                if (value < 10) speedTurn = value;
                else Debug.LogWarning("Попытка задать некорректную скорость поворота " + value);
            }
        }

        public float SpeedBack
        {
            get
            {
                return speedBack;
            }

            set
            {
                if (value < 10) speedBack = value;
                else Debug.LogWarning("Попытка задать некорректную скорость движения назад " + value);
            }
        }

        public float SpeedForward
        {
            get
            {
                return speedForward;
            }

            set
            {
                if (value < 10) speedForward = value;
                else Debug.LogWarning("Попытка задать некорректныую скорость движения вперед " + value);
            }
        }

        public int HoverHeight
        {
            get
            {
                return hoverHeight;
            }

            set
            {
                if (value < 20) hoverHeight = value;
                else Debug.LogWarning("Попытка задать некорректную высоту парения " + value);
            }
        }

        public Rigidbody ThisRigidbody
        {
            get { return thisRigidbody; }
        }

        public float MaxSpeed
        {
            get
            {
                return maxSpeed;
            }

            set
            {
                if (value < 100) maxSpeed = value;
                else Debug.LogWarning("Попытка задать некорректную максимальную скорость " + value);
            }
        }

        #endregion

        private void Start()
        {
            thisRigidbody = GetComponent<Rigidbody>();
        }

        /// <summary>
        /// Сдвинуть танк вперед.
        /// </summary>
        private void MoveForwardTank()
        {
                ThisRigidbody.AddRelativeForce(Vector3.right * -SpeedForward); 
        }

        /// <summary>
        /// Повернуть танк налево.
        /// </summary>
        private void TurnLeftTank()
        {
            ThisRigidbody.AddTorque(new Vector3(0, -SpeedTurn, 0));
        }

        /// <summary>
        /// Повернуть танк направо.
        /// </summary>
        private void TurnRightTank()
        {
            ThisRigidbody.AddTorque(new Vector3(0, SpeedTurn, 0));
        }

        /// <summary>
        /// Дать задний ход.
        /// </summary>
        private void MoveBackTank()
        {
            ThisRigidbody.AddRelativeForce(Vector3.right * SpeedBack);
        }

        private void HoverTank()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, HoverHeight))
            {
                 thisRigidbody.AddForce(0, 10, 0);
            }

            if (thisRigidbody.velocity.y < 1)
            {
                thisRigidbody.AddForce(0, -2, 0);
            }
        }

        private void StabilizationTank()
        {
            if (thisRigidbody.velocity.magnitude > MaxSpeed)
            {
                thisRigidbody.velocity = thisRigidbody.velocity.normalized * MaxSpeed;
            }
        }

        private void FixedUpdate()
        {
            HoverTank();
            StabilizationTank();
            //DebugMoveKeyboard();
            DebugVelocity();
        }

        private void DebugMoveKeyboard()
        {
            if (Input.GetKey(KeyCode.W)) MoveForwardTank();
            if (Input.GetKey(KeyCode.S)) MoveBackTank();
            if (Input.GetKey(KeyCode.A)) TurnLeftTank();
            if (Input.GetKey(KeyCode.D)) TurnRightTank();
        }

        private void DebugVelocity()
        {
            Debug.DrawRay(transform.position, thisRigidbody.velocity, Color.red);

            sX = thisRigidbody.velocity.x;
            sY = thisRigidbody.velocity.y;
            sZ = thisRigidbody.velocity.z;
        }
    
    }
}
