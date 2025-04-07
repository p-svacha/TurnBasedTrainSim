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
    private const float ZOOM_SPEED = 1f;
    private const float MIN_ZOOM_LEVEL = 2f;
    private const float INITIAL_ZOOM_LEVEL = 8f;
    private const float MAX_ZOOM_LEVEL = 20f;

    private Camera Camera;

    private bool InStrategicView;
    private float ZoomLevel;

    private void Awake()
    {
        Camera = GetComponent<Camera>();
        ZoomLevel = INITIAL_ZOOM_LEVEL;
        UpdateZoom();
    }

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

        if (Input.GetKeyDown(KeyCode.C)) ToggleStrategicView();

        if (Input.mouseScrollDelta.y < 0)
        {
            ZoomLevel += ZOOM_SPEED;
            if (ZoomLevel > MAX_ZOOM_LEVEL) ZoomLevel = MAX_ZOOM_LEVEL;
            UpdateZoom();
        }
        if (Input.mouseScrollDelta.y > 0)
        {
            ZoomLevel -= ZOOM_SPEED;
            if (ZoomLevel < MIN_ZOOM_LEVEL) ZoomLevel = MIN_ZOOM_LEVEL;
            UpdateZoom();
        }
    }

    private void UpdateZoom()
    {
        if (InStrategicView)
        {
            float orthoSize = ZoomLevel;
            Camera.orthographicSize = orthoSize;
        }
        else
        {
            float offsetYZ = ZoomLevel;
            transform.position = new Vector3(transform.position.x, offsetYZ, -(offsetYZ * 1.25f));
        }
    }
    
    private void ToggleStrategicView()
    {
        if (!InStrategicView) // Toggle into strategic view
        {
            Camera.orthographic = true;
            transform.position = new Vector3(transform.position.x, 10f, 0f);
            transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        }
        else // Toggle out of strategic view
        {
            Camera.orthographic = false;
            transform.rotation = Quaternion.Euler(35f, 0f, 0f);
        }

        InStrategicView = !InStrategicView;
        UpdateZoom();
    }
}
