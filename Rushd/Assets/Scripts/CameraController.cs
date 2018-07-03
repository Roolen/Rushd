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
                if (Vector3.Distance(transform.position, targetObject.position) > 15)
                    transform.Translate(targetObject.position * 0.01f);

            transform.position = new Vector3(transform.position.x, 10, transform.position.z);
        }

    }
}
