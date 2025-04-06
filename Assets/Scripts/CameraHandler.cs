using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Attach to camera
/// </summary>
public class CameraHandler : MonoBehaviour
{
    private const float SIDE_MOVE_SPEED = 5f;
    private const float SIDE_MOVE_SPEED_SHIFT = 10f;
    private const float ZOOM_SPEED = 200f;

    private bool InStrategicView;

    private void Update()
    {
        HandleInputs();
    }

    private void HandleInputs()
    {
        bool isHoldingShift = Input.GetKey(KeyCode.LeftShift);

        if(Input.GetKey(KeyCode.A))
        {
            float offsetX = isHoldingShift ? SIDE_MOVE_SPEED_SHIFT : SIDE_MOVE_SPEED;
            offsetX *= Time.deltaTime;
            transform.position += new Vector3(-offsetX, 0f, 0f);
        }
        if (Input.GetKey(KeyCode.D))
        {
            float offsetX = isHoldingShift ? SIDE_MOVE_SPEED_SHIFT : SIDE_MOVE_SPEED;
            offsetX *= Time.deltaTime;
            transform.position += new Vector3(offsetX, 0f, 0f);
        }

        if (Input.mouseScrollDelta.y < 0)
        {
            float offsetYZ = ZOOM_SPEED;
            offsetYZ *= Time.deltaTime;
            transform.position += new Vector3(0f, offsetYZ, -offsetYZ);
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            float offsetYZ = ZOOM_SPEED;
            offsetYZ *= Time.deltaTime;
            transform.position += new Vector3(0f, -offsetYZ, offsetYZ);
        }
    }
}
