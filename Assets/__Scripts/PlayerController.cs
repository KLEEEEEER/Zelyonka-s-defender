using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public int health;
    public GameState gameState;

    public float walkSpeed = 2;
    public float runSpeed = 6;
    public float gravity = -12;
    public float jumpHeight = 1;
    [Range(0,1)]
    public float airControlPercent;

    public Transform aimPosition;

    public float turnSmoothTime = 0.2f;
    float turnSmoothVelocity;
    public float speedSmoothTime = 0.1f;
    float speedSmoothVelocity;
    float currentSpeed;
    float velocityY;

    Animator animator;
    Transform cameraT;
    CharacterController controller;

    public bool isAiming = false;

    public GameObject gunPrefab;
    GameObject currentGun;
    public Transform gunHand;

    public Image Crosshair;

    public RectTransform healthbar;

    public Joystick joystick;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        cameraT = Camera.main.transform;
        controller = GetComponent<CharacterController>();

        if (currentGun == null)
        {
            currentGun = Instantiate<GameObject>(gunPrefab, gunHand);
            //currentGun = Instantiate<GameObject>(gunPrefab);
            //currentGun.transform.parent = gunHand;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 input = new Vector2(joystick.Horizontal, joystick.Vertical);
        Vector2 inputDir = input.normalized;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }

        //isAiming = (Input.GetKey(KeyCode.Mouse1)) ? true : false;

        if (!isAiming)
        {
            Crosshair.enabled = false;
            animator.SetLayerWeight(1, 0);
            if (inputDir != Vector2.zero)
            {
            
                    float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
                    transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
           
            }
        }
        else
        {
            Crosshair.enabled = true;
            /*if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                animator.Play("Armature|ShootPistol");
                Gun currentGunComponent = currentGun.GetComponent<Gun>();
                if (currentGunComponent != null)
                    currentGunComponent.Shoot();
            }*/
            animator.SetLayerWeight(1, 1);
            //transform.eulerAngles = cameraT.eulerAngles;
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, cameraT.eulerAngles.y, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime) / 4); ;
        }

        bool running = Input.GetKey(KeyCode.LeftShift);

        Move(inputDir, running);

        float animationSpeedPercent = ((running) ? currentSpeed / runSpeed : currentSpeed / walkSpeed * .5f) * inputDir.magnitude;
        animator.SetFloat("speedPercent", animationSpeedPercent, speedSmoothTime, Time.deltaTime);
    }

    void Move( Vector2 inputDir, bool running)
    {
        float targetSpeed = ((running) ? runSpeed : walkSpeed) * inputDir.magnitude;
        currentSpeed = Mathf.SmoothDamp(currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

        velocityY += Time.deltaTime * gravity;

        //transform.Translate(transform.forward * currentSpeed * Time.deltaTime, Space.World);

        Vector3 velocity;
        if (!isAiming)
        {
            velocity = transform.forward * currentSpeed + Vector3.up * velocityY;
        }
        else
        {
            velocity = transform.TransformDirection(new Vector3(inputDir.x, 0, inputDir.y)) * currentSpeed + Vector3.up * velocityY;
        }

        controller.Move(velocity * Time.deltaTime);

        currentSpeed = new Vector2(controller.velocity.x, controller.velocity.z).magnitude;

        if (controller.isGrounded)
        {
            velocityY = 0;
        }

    }

    public void Jump()
    {
        if (controller.isGrounded)
        {
            float jumpVelocity = Mathf.Sqrt(-2 * gravity * jumpHeight);
            velocityY = jumpVelocity;
        }
    }

    public void Aim()
    {
        isAiming = !isAiming;
    }

    public void Shoot()
    {
        animator.Play("Armature|ShootPistol");
        Gun currentGunComponent = currentGun.GetComponent<Gun>();
        if (currentGunComponent != null)
            currentGunComponent.Shoot();
    }

    float GetModifiedSmoothTime(float smoothTime)
    {
        if (controller.isGrounded)
        {
            return smoothTime;
        }
        if (airControlPercent == 0)
        {
            return float.MaxValue;
        }
        return smoothTime / airControlPercent;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        healthbar.sizeDelta = new Vector2(health * 2, healthbar.sizeDelta.y);
        if (health <= 0)
        {
            health = 0;
            gameState.GameOver();
            Destroy(gameObject);
        }
    }
}
