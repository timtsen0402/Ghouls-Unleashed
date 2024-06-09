using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameController;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Physics")]
    public CharacterController controller;
    public float walkSpeed = 15f;
    public float runSpeed = 30f;
    public float jumpHeight = 3f;
    public float gravity = -30f;

    [Header("Player Interaction")]
    public LayerMask groundMask;

    Transform groundCheck;
    float groundDistance = 0.4f;
    float speed;
    Vector3 velocity;
    bool isGround = false;


    private void Start()
    {
        groundCheck = GameObject.Find("GroundCheck").GetComponent<Transform>();
    }
    void Update()
    {
        isGround = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);
        if (isGround && velocity.y < 0f)
        {
            velocity.y = -2f;
        }


        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.forward * z;

        controller.Move(move * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && isGround)
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        WalkRun(walkSpeed, runSpeed);
    }
    void WalkRun(float walkSpeed, float runSpeed)
    {
        //Idle
        if (!Input.GetKey(KeyCode.W) && !Input.GetKey(KeyCode.A) &&
            !Input.GetKey(KeyCode.S) && !Input.GetKey(KeyCode.D))
        {
            FindObjectOfType<AudioManager>().Enable("runStep", false);
            FindObjectOfType<AudioManager>().Enable("walkStep", false);
            return;
        }

        //run
        if (Input.GetKey(KeyCode.LeftShift))
        {
            speed = runSpeed;
            FindObjectOfType<AudioManager>().Enable("runStep", true);
            FindObjectOfType<AudioManager>().Enable("walkStep", false);
        }
        //walk
        else
        {
            speed = walkSpeed;
            FindObjectOfType<AudioManager>().Enable("runStep", false);
            FindObjectOfType<AudioManager>().Enable("walkStep", true);
        }

    }
}
