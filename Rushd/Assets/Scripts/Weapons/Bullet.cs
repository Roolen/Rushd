using Assets.Scripts.Controllers;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float speed = 0;
    [SerializeField] private float radiusExplosion;
    [SerializeField] private float forceExplosion;

    private Vector3 startVector;
    private new Rigidbody rigidbody;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        ExplosionBullet();

        if (collision.gameObject.GetComponent<TankController>() != null)
        {
            var tc = collision.gameObject.GetComponent<TankController>();
            tc.SetDamage(20f);
        }

        Destroy(GetComponent<Light>());
        Destroy(this);
    }

    public void SetStartVector(Vector3 vector)
    {
        startVector = vector;
    }

    public void Shoot()
    {
        rigidbody.AddRelativeForce(startVector * (speed * 10));
    }

    private void ExplosionBullet()
    {
        var colliders = Physics.OverlapSphere(transform.position, radiusExplosion);
        foreach (Collider hit in colliders)
        {
            if (hit.GetComponent<Rigidbody>())
            {
                Rigidbody rib = hit.GetComponent<Rigidbody>();

                rib.AddExplosionForce(forceExplosion, transform.position, radiusExplosion, 3f);
            }
        }
    }
}
