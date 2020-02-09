using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    public CameraConfig cameraConfig;


    public float RotationSpeed = 1;
    public bool leftPivot;
    float mouseX, mouseY;
    float smoothXVelocity, smoothYVelocity, smoothX, smoothY;

    public Transform Target, Player, Pivot;

    [SerializeField]
    private ThirdPersonPlayerController playerController;

    public Transform Obstruction;
    float zoomSpeed = 2f;

    private Camera camera;

    void Start()
    {
        Obstruction = Target;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        camera = GetComponent<Camera>();
    }

    void Update()
    {
        HandlePosition();
        HandleRotation();
    }

    void HandlePosition()
    {
        float targetX = cameraConfig.normalX;
        float targetY = cameraConfig.normalY;
        float targetZ = cameraConfig.normalZ;

        if (playerController.isAiming)
        {
            targetX = cameraConfig.aimX;
            targetZ = cameraConfig.aimZ;
        }

        if (leftPivot)
        {
            targetX = -targetX;
        }

        Vector3 newPivotPosition = Pivot.localPosition;
        newPivotPosition.x = targetX;
        newPivotPosition.y = targetY;
        newPivotPosition.z = targetZ;

        float t = Time.deltaTime * cameraConfig.pivotSpeed;
        Pivot.localPosition = Vector3.Lerp(Pivot.localPosition, newPivotPosition, t);
    }

    void HandleRotation()
    {
        mouseX += Input.GetAxis("Mouse X") * RotationSpeed;
        mouseY += Input.GetAxis("Mouse Y") * RotationSpeed;

        mouseY = Mathf.Clamp(mouseY, cameraConfig.minAngle, cameraConfig.maxAngle);

        if (cameraConfig.turnSmooth > 0)
        {
            smoothX = Mathf.SmoothDamp(smoothX, mouseX, ref smoothXVelocity, cameraConfig.turnSmooth);
            smoothY = Mathf.SmoothDamp(smoothY, mouseY, ref smoothYVelocity, cameraConfig.turnSmooth);
        }
        else
        {
            smoothX = mouseX;
            smoothY = mouseY;
        }



        if (smoothY < 0)
        {
            camera.fieldOfView = cameraConfig.CenterCameraPositionFov;
        }
        else
        {
            float newFov = cameraConfig.CenterCameraPositionFov - ((cameraConfig.CenterCameraPositionFov - cameraConfig.LowCameraPositionFov) / 100) * ((100 * Mathf.Abs(smoothY)) / 60);
            camera.fieldOfView = newFov;
        }

        //print($"smoothY = {smoothY}");

        //transform.LookAt(Target);

        
        Player.rotation = Quaternion.Euler(0, smoothX, 0);
        Pivot.rotation = Quaternion.Euler(-smoothY, smoothX, 0);
    }
}
