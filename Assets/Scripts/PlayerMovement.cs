using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class PlayerMovementAdvanced : MonoBehaviour
{
    [Header("Movement")]
    private float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    public float groundDrag;
    public float wallrunSpeed;

    [Header("Jumping")]
    public float jumpForce;
    public float jumpCooldown;
    public float airMultiplier;
    bool readyToJump;
    public float coyoteTime; // Time buffer for jumping

    [Header("Crouching")]
    public float crouchSpeed;
    public float crouchYScale;
    private float startYScale;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;
    public KeyCode sprintKey = KeyCode.LeftShift;
    public KeyCode crouchKey = KeyCode.LeftControl;

    [Header("Ground Check")]
    public float playerHeight;
    public LayerMask whatIsGround;
    bool grounded;

    [Header("Slope Handling")]
    public float maxSlopeAngle;
    private RaycastHit slopeHit;
    private bool exitingSlope;


    public Transform orientation;

    float horizontalInput;
    float verticalInput;

    PlayerCam PC;
    

    Vector3 moveDirection;

    Rigidbody rb;

    public MovementState state;
    public enum MovementState
    {
        walking,
        sprinting,
        wallrunning,
        crouching,
        air
    }

    public bool wallrunning;
    float coyoteTimeCounter;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;

        readyToJump = true;

        startYScale = transform.localScale.y;
        PC = FindObjectOfType<PlayerCam>();

        if (PC == null)
        {
            Debug.LogError("ERROR: PlayerCam script not found! Make sure it is attached to the camera.");
        }
    }

    private void Update()
    {
        // Ground check
        bool wasGrounded = grounded; // Track previous grounded state
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, whatIsGround);

        MyInput();
        SpeedControl();
        StateHandler();

        // Handle drag and coyote time correctly
        if (grounded)
        {
            rb.drag = groundDrag;

            // Prevent instant double jump by resetting only if falling
            if (!wasGrounded)
            {
                coyoteTimeCounter = coyoteTime;
                readyToJump = true; // Reset jump ONLY when landing
            }
        }
        else
        {
            rb.drag = 0;
            coyoteTimeCounter -= Time.deltaTime; // Decrease only when not grounded
        }
    }


    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

        // when to jump
        //if (Input.GetKey(jumpKey) && readyToJump && grounded && coyoteTimeCounter > 0)
        if (Input.GetKey(jumpKey) && readyToJump && coyoteTimeCounter > 0)
        {
            readyToJump = false; // Prevent multiple jumps

            Jump();
            coyoteTimeCounter = 0; // Reset to prevent multiple jumps

            Invoke(nameof(ResetJump), jumpCooldown);
        }

        // start crouch
        if (Input.GetKeyDown(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5f, ForceMode.Impulse);
        }

        // stop crouch
        if (Input.GetKeyUp(crouchKey))
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    private void StateHandler()
    {
        // Mode - Wall Running
        if(wallrunning)
        {
            state = MovementState.wallrunning;
            moveSpeed = wallrunSpeed;
            PC.DoFov(100);
        }


        // Mode - Crouching
        if (Input.GetKey(crouchKey))
        {
            state = MovementState.crouching;
            moveSpeed = crouchSpeed;
            PC.DoFov(70);
        }

        // Mode - Sprinting
        else if (grounded && Input.GetKey(sprintKey))
        {
            state = MovementState.sprinting;
            moveSpeed = sprintSpeed;
            PC.DoFov(90);
        }

        // Mode - Walking
        else if (grounded)
        {
            state = MovementState.walking;
            moveSpeed = walkSpeed;
            if (verticalInput < 0) // If pressing "S"
            {
                moveSpeed *= 0.7f; // Reduce speed to 70% of normal speed
            }
            PC.DoFov(80);
        }

        // Mode - Air
        else
        {
            state = MovementState.air;
        }
    }

    private void MovePlayer()
    {
        // Calculate movement direction
        moveDirection = orientation.forward * verticalInput + orientation.right * horizontalInput;

        // Reduce speed when moving backward (both on ground and in air)
        float adjustedSpeed = moveSpeed;
        if (verticalInput < 0) // If pressing "S"
        {
            adjustedSpeed *= 0.7f; // Reduce speed by 30%
        }

        // On slope
        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * adjustedSpeed * 20f, ForceMode.Force);

            if (rb.velocity.y > 0)
                rb.AddForce(Vector3.down * 80f, ForceMode.Force);
        }
        // On ground
        else if (grounded)
        {
            rb.AddForce(moveDirection.normalized * adjustedSpeed * 10f, ForceMode.Force);
        }
        // In air (apply airMultiplier)
        else
        {
            rb.AddForce(moveDirection.normalized * adjustedSpeed * 10f * airMultiplier, ForceMode.Force);
        }

        // Turn gravity off while on a slope
        if (!wallrunning) rb.useGravity = !OnSlope();
    }


    private void SpeedControl()
    {
        // limiting speed on slope
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
                rb.velocity = rb.velocity.normalized * moveSpeed;
        }

        // limiting speed on ground or in air
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

            // limit velocity if needed
            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    private void Jump()
    {
        exitingSlope = true;

        // reset y velocity
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
        coyoteTimeCounter = 0;

    }
    private void ResetJump()
    {
        readyToJump = true;

        exitingSlope = false;
    }

    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }

        return false;
    }

    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }
}