﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    public bool lockCursor;
    public float Sensitivity = 10f;
    public Transform Target;
    public float DstFromTraget = 2;

    public float rotationSmoothTime = .12f;
    public float aimSmoothTime = 1.0f;
    Vector3 rotationSmoothVelocity;
    Vector3 currentRotation;

    public Vector2 pitchMinMax = new Vector2(-40, 85);

    float yaw;
    float pitch;

    public PlayerController playerController;
    public Transform aimPosition;

    Vector3 aimCurrentSmoothVelocity;

    // Start is called before the first frame update
    void Start()
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    // Update is called once per frame
    void LateUpdate()
    {
        yaw += Input.GetAxis("Mouse X") * Sensitivity;
        pitch -= Input.GetAxis("Mouse Y") * Sensitivity;
        pitch = Mathf.Clamp(pitch, pitchMinMax.x, pitchMinMax.y);

        currentRotation = Vector3.SmoothDamp(currentRotation, new Vector3(pitch, yaw), ref rotationSmoothVelocity, rotationSmoothTime);
        transform.eulerAngles = currentRotation;

        if (!playerController.isAiming)
        {
            //transform.position = Target.position - transform.forward * DstFromTraget;
            Vector3 neededTargetPosition = Target.position - transform.forward * DstFromTraget;
            transform.position = Vector3.SmoothDamp(transform.position, neededTargetPosition, ref aimCurrentSmoothVelocity, aimSmoothTime);
        }
        else
        {
            //transform.position = aimPosition.position;
            transform.position = Vector3.SmoothDamp(transform.position, aimPosition.position, ref aimCurrentSmoothVelocity, aimSmoothTime);
        }
    }
}
