using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankController : MonoBehaviour
{
    public float HoverZ;
    public Rigidbody rb;

    private void MoveForvardTank()
    {

    }

    private void TurnLeftTank()
    {

    }

    private void TurnRightTank()
    {

    }

    private void MoveBackTank()
    {

    }

    private void HoverTank()
    {
        rb = GetComponent<Rigidbody>();
        rb.AddForce(transform.up * HoverZ);
        if (HoverZ == 30)
            HoverZ = 0;
        else if (HoverZ == -30)
            HoverZ = 0;
    }

    private void StabilizationTank()
    {

     
    
    }


    void FixedUpdate()
    {
        HoverTank();
    }

    





}
