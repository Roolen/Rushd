using System.Collections;
using System.Collections.Generic;
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

    public void SetStartVector(Vector3 vector)
    {
        startVector = vector;
    }

    public void Shoot()
    {
        rigidbody.AddRelativeForce(startVector * (speed * 10));
    }
}
