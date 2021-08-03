using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //config
    [SerializeField] float crouchSpeed = 5f;
    [SerializeField] float walkSpeed = 15f;
    [SerializeField] float runSpeed = 25f;
    [SerializeField] float jumpSpeed = 5f;

    //states
    public bool grounded = false;
    bool isCrouching;
    bool isWalking;
    bool isRunning;
    bool isStanding;

    //cached components
    Rigidbody myRigidBody;
    Animator myAnimator;

    void Start()
    {
        myRigidBody = GetComponent<Rigidbody>();
        myAnimator = GetComponent<Animator>();
    }

    private void FixedUpdate()
    {
        Movement();
        Jump();
    }


    private void Movement()
    {
        float controlThrow = Input.GetAxis("Vertical");
        if (controlThrow != 0 && Input.GetKey(KeyCode.LeftShift))
        {
            Vector3 playerVelocity = new Vector3(myRigidBody.velocity.x, myRigidBody.velocity.y, Mathf.Abs(controlThrow * runSpeed));
            myRigidBody.velocity = playerVelocity;
            isCrouching = false;
            isWalking = false;
            isStanding = false;
            isRunning = true;
        }
        else if (controlThrow != 0 && Input.GetKey(KeyCode.LeftControl))
        {
            Vector3 playerVelocity = new Vector3(myRigidBody.velocity.x, myRigidBody.velocity.y, Mathf.Abs(controlThrow * crouchSpeed));
            myRigidBody.velocity = playerVelocity;
            isCrouching = true;
            isWalking = false;
            isStanding = false;
            isRunning = false;
        }
        else if (controlThrow != 0)
        {
            Vector3 playerVelocity = new Vector3(myRigidBody.velocity.x, myRigidBody.velocity.y, Mathf.Abs(controlThrow * walkSpeed));
            myRigidBody.velocity = playerVelocity;
            isCrouching = false;
            isWalking = true;
            isStanding = false;
            isRunning = false;
        }
        else
        {
            isCrouching = false;
            isWalking = false;
            isStanding = true;
            isRunning = false;
        }
        // Set Animator Parameters:
        myAnimator.SetBool("isCrouching", isCrouching);
        myAnimator.SetBool("isWalking", isWalking);
        myAnimator.SetBool("isStanding", isStanding);
        myAnimator.SetBool("isRunning", isRunning);
    }
    
    private void Jump()
    {
        if (!grounded && myRigidBody.velocity.y == 0)
        {
            grounded = true;
        }
        if (grounded && Input.GetButton("Jump"))
        {
            grounded = false;
            Vector3 jumpVelocity = new Vector3(0f, jumpSpeed);
            myRigidBody.velocity += jumpVelocity;
            myAnimator.SetTrigger("hasJumped");
        }
    }

}
