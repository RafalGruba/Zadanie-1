using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    //config
    [Header("max move speed + jump force")]
    [SerializeField] float crouchSpeed = 2f;
    [SerializeField] float walkSpeed = 15f;
    [SerializeField] float runSpeed = 25f;
    [SerializeField] float jumpForce = 5f;
    [Header("acceleration and brake")]
    [SerializeField] float walkTimeZeroToMax = 2.5f;
    [SerializeField] float runTimeZeroToMax = 2.5f;
    [SerializeField] float brakeForce = 30f;
    float timeBrakeToZero = 2f;
    float walkAccelRatePerSec;
    float runAccelRatePerSec;
    float brakeRatePerSec;
    float forwardVelocity;

    //states
    bool grounded = false;
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
        walkAccelRatePerSec = walkSpeed / walkTimeZeroToMax;
        runAccelRatePerSec = runSpeed / runTimeZeroToMax;
        brakeRatePerSec = -brakeForce / timeBrakeToZero;
        forwardVelocity = 0f;
    }

    private void FixedUpdate()
    {
        Movement();
        Jump();
    }


    private void Movement()
    {
        float controlThrow = Input.GetAxis("Vertical");
        controlThrow = Mathf.Abs(controlThrow);
        if (controlThrow != 0 && Input.GetKey(KeyCode.LeftShift))
        {
            forwardVelocity += walkAccelRatePerSec * Time.deltaTime;
            forwardVelocity = Mathf.Min(forwardVelocity, runSpeed);
            myRigidBody.velocity = transform.forward * (controlThrow * forwardVelocity);
            isCrouching = false; isWalking = false; isStanding = false; isRunning = true;
        }
        else if (controlThrow != 0 && Input.GetKey(KeyCode.LeftControl))
        {
            myRigidBody.velocity = transform.forward * (controlThrow * crouchSpeed);
            isCrouching = true; isWalking = false; isStanding = false; isRunning = false;
        }
        else if (controlThrow != 0)
        {
            forwardVelocity += runAccelRatePerSec * Time.deltaTime;
            forwardVelocity = Mathf.Min(forwardVelocity, walkSpeed);
            myRigidBody.velocity = transform.forward * (controlThrow * forwardVelocity);
            isCrouching = false; isWalking = true; isStanding = false; isRunning = false;
        }
        else
        {
            isCrouching = false; isWalking = false; isStanding = true; isRunning = false;
        }
        if (Input.GetKey(KeyCode.Z))
        {
            forwardVelocity += brakeRatePerSec * Time.deltaTime;
            forwardVelocity = Mathf.Max(forwardVelocity, 0);
            myRigidBody.velocity = transform.forward * (controlThrow * forwardVelocity);
        }
        forwardVelocity = myRigidBody.velocity.magnitude;
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
            Vector3 jumpVelocity = new Vector3(0f, jumpForce);
            myRigidBody.velocity += jumpVelocity;
            myAnimator.SetTrigger("hasJumped");
        }
    }

}
