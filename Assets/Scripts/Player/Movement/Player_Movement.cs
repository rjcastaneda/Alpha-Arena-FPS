using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    // Initialize Variables
    public Transform playerCamera;
    public Transform orientation;

    private Rigidbody rb;

    // Sensitivity Variables
    private float xRotation;
    private float sensitivity = 25f;
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

        // Used to tune movement
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
        if (y > 0 && yMagnitude < maxSpeed) y = 0;
        if (y < 0 && yMagnitude < -maxSpeed) y = 0;

        float multiplier = 1f;
        float multiplierV = 1f;

        // Air movement scalings
        // Jumping makes us go faster because go fast = fun
        if (!grounded)
        {
            multiplier = 1.1f;
            multiplierV = 1.1f;
        }

        // Movement while sliding
        // (Will later tune this value)
        if (grounded && crouching) multiplierV = 0f;

        // Apply forces to the player
        rb.AddForce(orientation.transform.forward * y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * x * moveSpeed * Time.deltaTime * multiplier);
    }
}
