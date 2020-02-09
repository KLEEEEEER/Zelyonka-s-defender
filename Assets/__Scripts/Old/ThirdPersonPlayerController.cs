using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonPlayerController : MonoBehaviour
{
    public float Speed;
    public float JumpHeight;
    public float Gravity = -9.81f;

    private float hor;
    private float ver;

    public CharacterController characterController;

    public Transform GroundCheck;
    public float GroundDistance = 0.4f;
    public LayerMask GroundMask;

    private Vector3 velocity;

    public bool isGrounded, isAiming, isSprinting;

    public bool isAimHeld = false;

    public Animator anim;

    void Update()
    {
        PlayerMovement();
    }

    void PlayerMovement()
    {
        isGrounded = Physics.CheckSphere(GroundCheck.position, GroundDistance, GroundMask);

        //print($"isGrounded = {isGrounded.ToString()}");

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        hor = Input.GetAxis("Horizontal");
        ver = Input.GetAxis("Vertical");

        if (isAiming)
        {
            hor /= 3;
            ver /= 3;
            Vector3 playerMovement = transform.right * hor + transform.forward * ver;
            characterController.Move(playerMovement.normalized * (Speed / 3) * Time.deltaTime);
        }
        else
        {
            Vector3 playerMovement = transform.right * hor + transform.forward * ver;
            characterController.Move(playerMovement.normalized * Speed * Time.deltaTime);
        }


        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            velocity.y = Mathf.Sqrt(JumpHeight * -2f * Gravity);
        }

        if (Input.GetButton("Fire2"))
        {
            isAiming = true;
        }

        if (Input.GetButtonUp("Fire2"))
        {
            isAiming = false;
        }

        velocity.y += Gravity * Time.deltaTime;

        anim.SetFloat("vertical", ver, 0.15f, Time.deltaTime);

        characterController.Move(velocity * Time.deltaTime);
    }
}
