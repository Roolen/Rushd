using UnityEngine;

namespace Assets.Scripts
{
    public class TankBot : MonoBehaviour
    {
        public GameObject defenseTarget;
        public Transform target;

        private void Start()
        {
            target = GameObject.Find("PlayerTank").transform;
        }

        private void FixedUpdate()
        {
            Quaternion currRot = transform.rotation;
            GetComponent<Transform>().LookAt(target);
        }
    }
}
