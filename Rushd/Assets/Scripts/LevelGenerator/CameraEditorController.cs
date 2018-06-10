using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.LevelGenerator;
using UnityEngine;

public class CameraEditorController : MonoBehaviour
{
    public Rigidbody mainCamera;
    public int speedForCamera;
    public int speedScrollWhell;
    private LevelData date;

    private int zLimit;
    private int xLimit;
    private int speedLimit = 1;

    private void Start()
    {
        date = FindObjectOfType<LevelData>();
    }

    private void FixedUpdate()
    {
        xLimit = date.Height * 10;
        zLimit = date.Weight * 10;

        mainCamera.transform.Translate(Input.GetAxis("Horizontal") * speedForCamera, 0, 0);
        mainCamera.transform.Translate(0, 0, Input.GetAxis("Vertical") * speedForCamera, Space.World);
        mainCamera.transform.Translate(0, Input.GetAxis("Mouse ScrollWheel") * -speedScrollWhell * Time.deltaTime, 0, Space.World);

        if (mainCamera.transform.position.x > 50 + xLimit) { mainCamera.transform.Translate(-speedLimit, 0, 0, Space.World); }
        if (mainCamera.transform.position.x < 0) { mainCamera.transform.Translate(speedLimit, 0, 0, Space.World); }

        if (mainCamera.transform.position.z > 50 + zLimit) { mainCamera.transform.Translate(0, 0, -speedLimit, Space.World); }
        if (mainCamera.transform.position.z < -50) { mainCamera.transform.Translate(0, 0, speedLimit, Space.World); }

        if (mainCamera.transform.position.y > 90 + zLimit) { mainCamera.transform.Translate(0, -speedLimit, 0, Space.World); }
        if (mainCamera.transform.position.y < 40) { mainCamera.transform.Translate(0, speedLimit, 0, Space.World); }
    }


}
