using UnityEngine;

namespace Assets.Scripts
{
    public class CameraController : MonoBehaviour
    {
        public Transform targetObject;
        private int timer;

        private void FixedUpdate()
        {
            GetComponent<Transform>().LookAt(targetObject);

            transform.position = Vector3.Lerp(transform.position, targetObject.position, Time.deltaTime * 1);

            transform.position = new Vector3(transform.position.x, 10, transform.position.z);
        }

    }
}
