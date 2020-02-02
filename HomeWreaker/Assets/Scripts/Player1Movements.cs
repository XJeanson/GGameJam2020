using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;

public class Player1Movements : MonoBehaviour
{
    CharacterController characterController;

    public float speed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;

    private Vector3 moveDirection = Vector3.zero;
    private float horizontal;
    private float vertical;
    private Vector3 direction;
    private Animator anim;

    void Start()
    {
        characterController = GetComponent<CharacterController>();
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        horizontal = Input.GetAxis("P1Horizontal");
        vertical = Input.GetAxis("P1Vertical");
        if (characterController.isGrounded)
        {
            // We are grounded, so recalculate
            // move direction directly from axes

            //Debug.Log(Input.GetAxis("P1Horizontal"));
            //Debug.Log(Input.GetAxis("P1Vertical"));

            direction = new Vector3(horizontal, 0, vertical);

            if ((Input.GetAxis("P1Horizontal") > 0.05 || Input.GetAxis("P1Horizontal") < -0.05) &&
                (Input.GetAxis("P1Vertical") > 0.05 || Input.GetAxis("P1Vertical") < -0.05))
                moveDirection = new Vector3(Input.GetAxis("P1Horizontal"), 0.0f, Input.GetAxis("P1Vertical"));
            else
            {
                moveDirection = new Vector3(0.0f, 0.0f, 0.0f);
                horizontal = 0f;
                vertical = 0f;
            }
            moveDirection *= speed;

            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }
        }

        // Apply gravity. Gravity is multiplied by deltaTime twice (once here, and once below
        // when the moveDirection is multiplied by deltaTime). This is because gravity should be applied
        // as an acceleration (ms^-2)
        moveDirection.y -= gravity * Time.deltaTime;

        if(vertical > 0.05)
            anim.SetFloat("Speed", vertical);
        if (vertical < -0.05)
            anim.SetFloat("Speed", vertical * -1);
        if (horizontal > 0.05)
            anim.SetFloat("Speed", horizontal);
        if (horizontal < -0.05)
            anim.SetFloat("Speed", horizontal * -1);
        if (horizontal == 0f && vertical == 0f)
            anim.SetFloat("Speed", 0f);

        // Move the controller
        characterController.Move(moveDirection * Time.deltaTime);
        characterController.transform.LookAt(transform.position + direction);

        
    }
}
