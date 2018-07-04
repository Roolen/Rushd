using UnityEngine;

namespace Assets.Scripts
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [RequireComponent(typeof(Rigidbody))]
    public class TankController : MonoBehaviour
    {
        public Vector3 velocityNow;

        [SerializeField] private float speedTurn;

        [SerializeField] private float speedBack;

        [SerializeField] private float speedForward;

        [SerializeField] private float maxSpeed;

        [SerializeField] private int hoverHeight;

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
        public void MoveForwardTank()
        {
                ThisRigidbody.AddRelativeForce(Vector3.forward * SpeedForward); 
        }

        /// <summary>
        /// Повернуть танк налево.
        /// </summary>
        public void TurnLeftTank()
        {
            ThisRigidbody.AddTorque(new Vector3(0, -SpeedTurn, 0));
        }

        /// <summary>
        /// Повернуть танк направо.
        /// </summary>
        public void TurnRightTank()
        {
            ThisRigidbody.AddTorque(new Vector3(0, SpeedTurn, 0));
        }

        /// <summary>
        /// Дать задний ход.
        /// </summary>
        public void MoveBackTank()
        {
            ThisRigidbody.AddRelativeForce(Vector3.back * SpeedBack);
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
            DebugVelocity();
        }

        private void DebugVelocity()
        {
            Debug.DrawRay(transform.position, thisRigidbody.velocity, Color.red);

            velocityNow = thisRigidbody.velocity;
        }
    
    }
}
