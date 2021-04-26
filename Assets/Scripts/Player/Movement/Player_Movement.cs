using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    // Initialize Variables
    public Transform playerCamera;
    public Transform orientation;

    private Rigidbody rb;

    // Sensitivity Variables
    private float xRotation;
    private float sensitivity = 50f;
    private float sensMultiplier = 1f;

    // Movement Variables
    public float moveSpeed = 4500;
    public float maxSpeed = 20;
    public bool grounded;
    public LayerMask groundLayer;
    public float counterMovement = 0.175f;
    private float threshold = 0.01f;
    public float maxSlopeAngle = 35f;
    public float gravityScale = 10;

    // Jumping
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    public float jumpForce = 550f;

    // Crouch and Sliding Variables
    private Vector3 crouchScale = new Vector3(1, 0.55f, 1); // Y-value is changable for how we want how short/tall our crouch to be
    private Vector3 playerScale;
    public float slideThreshold = 0.5f;
    public float slideForce = 400;
    public float slideCounterMovement = 0.2f;
    public float slideVerticalSpeed = 3000;

    // Player Input Variables
    float x, y;
    bool jumping;
    bool sprinting;
    bool crouching;

    // Vector Variables (used for slide movement mechanics)
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        playerScale = transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        UserInput();
        Look();
    }

    // Handles and assigns user movement inputs
    private void UserInput()
    {
        x = Input.GetAxisRaw("Horizontal");
        y = Input.GetAxisRaw("Vertical");
        jumping = Input.GetButton("Jump");
        crouching = Input.GetButton("Crouch");

        if (Input.GetButtonDown("Crouch"))
        {
            StartCrouch();
        }

        if (Input.GetButtonUp("Crouch"))
        {
            StopCrouch();
        }
    }

    private void StartCrouch()
    {
        // Scale player down to crouch size
        // TODO (if time permits): Animation to match crouch instead of "squishing" the player downwards to simulate a crouch
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.55f, transform.position.z);
        
        // If player's velocity is > slideThreshold, initiate a slide. SlideThreshold & SlideForce changeable in editor 
        if (rb.velocity.magnitude > slideThreshold)
        {
            if (grounded)
            {
                rb.AddForce(orientation.transform.forward * slideForce);
            }
        }
    }

    private void StopCrouch()
    {
        // Re-scale player back to normal standing size
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.55f, transform.position.z);
    }

    private void Movement()
    {
        // Apply our game's own "Gravity Scale"
        rb.AddForce(Vector3.down * Time.deltaTime * gravityScale);

        // Find velocity relative to how their camera is moving (B-Hopping, Air Strafes)
        Vector2 magnitude = FindVelocityRelativeToLook();
        float xMagnitude = magnitude.x;
        float yMagnitude = magnitude.y;

        // Used to tune movement friction
        CounterMovement(x, y, magnitude);

        // Allows player to jump multiple times
        // Conditions: Ready to Jump AND is already Jumping
        // TODO: Tune so that we are only able to double-jump
        if (readyToJump && jumping)
        {
            Jump();
        }

        float maxSpeed = this.maxSpeed;

        // Creates a downward sliding force if the player is on a slope
        // Adjusted through slideVerticalSpeed
        if (crouching && grounded && readyToJump)
        {
            rb.AddForce(Vector3.down * Time.deltaTime * slideVerticalSpeed);
            return;
        }

        // Conditions for keeping player within speed limits
        // One liners for legibility
        if (x > 0 && xMagnitude > maxSpeed) x = 0;
        if (x < 0 && xMagnitude < -maxSpeed) x = 0;
        if (y > 0 && yMagnitude > maxSpeed) y = 0;
        if (y < 0 && yMagnitude < -maxSpeed) y = 0;

        float multiplier = 1f;
        float multiplierV = 1f;

        // Air movement scalings
        if (!grounded)
        {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }

        // Movement while sliding
        // (Will later tune this value)
        if (grounded && crouching) multiplierV = 0f;

        // Apply forces to the player
        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);
    }

    private void Jump()
    {
        if (grounded && readyToJump)
        {
            // Cannot temporarily jump
            readyToJump = false;

            // Apply forces to jump
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);

            // Handles jumping while falling
            // Used to reset your y-velocity or else jumps will feel like they are trying to cancel gravity's downward force
            Vector3 magnitude = rb.velocity;
            if (rb.velocity.y < 0.5f)
            {
                rb.velocity = new Vector3(magnitude.x, 0, magnitude.z);
            }
            else if (rb.velocity.y > 0)
            {
                rb.velocity = new Vector3(magnitude.x, magnitude.y / 2, magnitude.z);
            }

            // Begin jump cooldown
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    private float desiredX;
    private void Look()
    {
        // Initialize mouse variables
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.fixedDeltaTime * sensMultiplier;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.fixedDeltaTime * sensMultiplier;

        // Find player's current look position
        Vector3 rotation = playerCamera.transform.localRotation.eulerAngles;
        desiredX = rotation.y + mouseX;

        // Handles mouse rotation
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        // Apply the rotations
        playerCamera.transform.localRotation = Quaternion.Euler(xRotation, desiredX, 0);
        orientation.transform.localRotation = Quaternion.Euler(0, desiredX, 0);
    }

    // Friction
    private void CounterMovement(float x, float y, Vector2 magnitude)
    {
        // Jumping handled elsewhere
        if (!grounded || jumping) return;

        // Slows down the player when crouching
        if (crouching)
        {
            rb.AddForce(moveSpeed * Time.deltaTime * -rb.velocity.normalized * slideCounterMovement);
        }

        // Counter movement
        if (Math.Abs(magnitude.x) > threshold && Math.Abs(x) < 0.05f || (magnitude.x < -threshold && x > 0) || (magnitude.x > threshold && x < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.right * Time.deltaTime * -magnitude.x * counterMovement);
        }
        if (Math.Abs(magnitude.y) > threshold && Math.Abs(y) < 0.05f || (magnitude.y < -threshold && y > 0) || (magnitude.y > threshold && y < 0))
        {
            rb.AddForce(moveSpeed * orientation.transform.forward * Time.deltaTime * -magnitude.y * counterMovement);
        }

        // Inhibits "Diagonal" speed boosts
        if (Mathf.Sqrt((Mathf.Pow(rb.velocity.x, 2) + Mathf.Pow(rb.velocity.z, 2))) > maxSpeed)
        {
            float fallSpeed = rb.velocity.y;
            Vector3 normal = rb.velocity.normalized * maxSpeed;
            rb.velocity = new Vector3(normal.x, fallSpeed, normal.z);
        }
    }

    // Find velocity relative to where the player is moving their camera
    // Used for creating cool movement like B-Hopping and Air Strafes
    private Vector2 FindVelocityRelativeToLook()
    {
        float lookAngle = orientation.transform.eulerAngles.y;
        float moveAngle = Mathf.Atan2(rb.velocity.x, rb.velocity.z) * Mathf.Rad2Deg;

        float u = Mathf.DeltaAngle(lookAngle, moveAngle);
        float v = 90 - u;

        float magnitude = rb.velocity.magnitude;
        float xMagnitude = magnitude * Mathf.Cos(v * Mathf.Deg2Rad);
        float yMagnitude = magnitude * Mathf.Cos(u * Mathf.Deg2Rad);

        return new Vector2(xMagnitude, yMagnitude);
    }

    // Figures out characteristic of the floor we are standing on
    private bool IsFloor(Vector3 vector)
    {
        float angle = Vector3.Angle(Vector3.up, vector);
        return angle < maxSlopeAngle;
    }

    private bool cancellingGrounded;
    private void OnCollisionStay(Collision collision)
    {
        // Ensures that we are using ground layers
        int layer = collision.gameObject.layer;
        if (groundLayer != (groundLayer | (1 << layer))) return;

        // Checks every "ground" layer surface
        for (int i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.contacts[i].normal;
            // Is the player touching the floor?
            if (IsFloor(normal))
            {
                grounded = true;
                cancellingGrounded = false;
                normalVector = normal;
                CancelInvoke(nameof(StopGrounded));
            }
        }

        // Fixes ground/wall collision detection
        float delay = 3f;
        if (!cancellingGrounded)
        {
            cancellingGrounded = true;
            Invoke(nameof(StopGrounded), Time.deltaTime * delay);
        }
    }

    private void StopGrounded()
    {
        grounded = false;
    }
}
