using UnityEngine;

namespace Assets.Scripts
{
    // ReSharper disable once ClassNeverInstantiated.Global
    [RequireComponent(typeof(Rigidbody))]
    public class TankController : MonoBehaviour
    {
        [SerializeField]
        private float speedTurn;

        [SerializeField]
        private float speedBack;

        [SerializeField]
        private float speedForward;

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

        #endregion

        private void Start()
        {
            thisRigidbody = GetComponent<Rigidbody>();
        }

        private void MoveForwardTank()
        {
            ThisRigidbody.AddRelativeForce(Vector3.right * -SpeedForward, ForceMode.Force); 
        }

        private void TurnLeftTank()
        {
            ThisRigidbody.AddTorque(new Vector3(0, -SpeedTurn, 0));
        }

        private void TurnRightTank()
        {
            ThisRigidbody.AddTorque(new Vector3(0, SpeedTurn, 0));
        }

        private void MoveBackTank()
        {
            ThisRigidbody.AddRelativeForce(Vector3.right * SpeedBack, ForceMode.Force);
        }

        private void HoverTank()
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, HoverHeight))
            {
                if (thisRigidbody.GetPointVelocity(gameObject.transform.position).y < 5 )
                thisRigidbody.AddForce(0, 2, 0);
            }
      
        }

        private void StabilizationTank()
        {

     
    
        }

        void FixedUpdate()
        {
            HoverTank();

            Debug.Log(thisRigidbody.GetPointVelocity(gameObject.transform.position));
        }
    
    }
}
