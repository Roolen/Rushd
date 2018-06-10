using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public float turnRight;
    public float turnLeft;
    public float moveBack;
    public float moveForvard;
    public int hoverHigh;
    public Rigidbody rb;

    private void MoveForvardTank()
    {
        rb.AddForce(0, 0, moveForvard); 
    }

    private void TurnLeftTank()
    {
        float turn = Input.GetAxis("Horizontal");
        rb.AddTorque(new Vector3(0, turnLeft, 0));
    }

    private void TurnRightTank()
    {
        float turn = Input.GetAxis("Horizontal");
        rb.AddTorque(new Vector3(0, turnRight, 0));
    }

    private void MoveBackTank()
    {
        rb.AddForce(0, 0, moveBack);
    }

    private void HoverTank()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.down), out hit, hoverHigh))
        {
            GetComponent<Rigidbody>().AddForce(0, 2, 0);
        }
      
    }

    private void StabilizationTank()
    {

     
    
    }


    void FixedUpdate()
    {
        HoverTank();
    }

    





}
