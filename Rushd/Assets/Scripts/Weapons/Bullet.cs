using Assets.Scripts.Controllers;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 0;

    private Vector3 startVector;
    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<TankController>() != null)
        {
            var tc = collision.gameObject.GetComponent<TankController>();
            tc.SetDamage(20f);
        }
    }

    public void SetStartVector(Vector3 vector)
    {
        startVector = vector;
    }

    public void Shoot()
    {
        rigidbody.AddRelativeForce(startVector * (speed * 10));
    }
}
