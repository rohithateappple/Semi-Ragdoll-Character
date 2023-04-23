using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public CharacterController controller;
    public Transform cam;

    public float speed = 6;
    public float gravity = -9.81f;
    public float jumpHeight = 3;

    public bool rotateCheck = false;

    public float walkBobAmount = 0.05f;
    public float walkBobSpeed = 14f;
    public float runBobAmount = 0.1f;
    public float runBobSpeed = 18f;

    public Transform orientation;

    float defaultYPos;

    float timer;

    Vector3 velocity;
    bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    float x;
    float y;
    public float sensitivity = 5.0f;

    Vector3 rotate;
    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;

    float smooth = 5.0f;
    float tiltAngle = 60.0f;

    private void Awake()
    {
        defaultYPos = transform.localPosition.y;
        Cursor.lockState = CursorLockMode.Locked;
        
    }


    private void Update()
    {
        float horizontal_1 = Input.GetAxisRaw("Horizontal");
        float vertical_1 = Input.GetAxisRaw("Vertical");

        Vector3 direction_1 = new Vector3(horizontal_1, 0f, vertical_1).normalized;


        float targetAngle_1 = Mathf.Atan2(direction_1.x, direction_1.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle_1, ref turnSmoothVelocity, turnSmoothTime);

        if (rotateCheck)
        {
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }

        //
    }
    void FixedUpdate()
    {
        //jump
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        //gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
        //walk
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;

        if (direction.magnitude >= 0.1f)
        {
            
            timer += Time.deltaTime * walkBobSpeed;
      
            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

            float yVal = defaultYPos + Mathf.Sin(timer) * walkBobAmount;            

            controller.Move(moveDir.normalized * speed * Time.deltaTime);

            transform.localPosition = new Vector3(transform.localPosition.x,
            yVal, transform.localPosition.z);

        }
        else
        {
            transform.localPosition = new Vector3(transform.localPosition.x,
            defaultYPos, transform.localPosition.z);
        }


    }
}

