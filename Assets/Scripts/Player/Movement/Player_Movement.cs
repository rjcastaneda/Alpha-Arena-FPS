using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(CharacterController))]
public class Player_Movement : MonoBehaviourPunCallbacks
{
    // Class for applying different movement behaviors for ground, air, strafe
    [System.Serializable]
    public class MovementSettings
    {
        public float MaxSpeed;
        public float Acceleration;
        public float Deceleration;

        public MovementSettings(float maxSpeed, float acceleration, float deceleration)
        {
            MaxSpeed = maxSpeed;
            Acceleration = acceleration;
            Deceleration = deceleration;
        }
    }

    // Keep everything private, since everything is going to be networked, public variables can be easily accessed at will
    [Header("Aiming")]
    [SerializeField] private Camera Camera;
    [SerializeField] private Player_MouseLook MouseLook = new Player_MouseLook();

    // Default Quake 3 movement variables converted to unity
    [Header("Movement")]
    [SerializeField] private float Friction = 6;
    [SerializeField] private float Gravity = 20;
    [SerializeField] private float JumpForce = 8;
    [Tooltip("Automatically jump when holding jump button")]
    [SerializeField] private bool AutoBunnyHop = false;
    [Tooltip("Air control precision")]
    [SerializeField] private float PlayerAirControl = 0.3f;
    [SerializeField] private MovementSettings GroundSettings = new MovementSettings(7, 14, 10);
    [SerializeField] private MovementSettings AirSettings = new MovementSettings(7, 2, 2);
    [SerializeField] private MovementSettings StrafeSettings = new MovementSettings(1, 50, 50);

    // Return of player's speed
    public float Speed
    {
        get
        {
            return Player.velocity.magnitude;
        }
    }

    private CharacterController Player;
    private Vector3 MoveDirectionNormal = Vector3.zero;
    private Vector3 PlayerVelocity = Vector3.zero;

    private bool IsJumpQueued = false;

    private float PlayerFriction = 0;

    private Vector3 MoveInput;
    private Transform PlayerTransform;
    private Transform PlayerCamera;

    private bool crouching;
    private float crouchValue = 1.8f;
    private float standValue = 3.5f;
    private float currentHeightValue = 3.5f;
    private float crouchCenterValue = -0.85f;
    private Vector3 crouchScale = new Vector3(1, 0.55f, 1);
    private Vector3 playerScale;

    private PhotonView PV;

    private void Start()
    {
        // Initialize user
        PlayerTransform = transform;
        //PV = GetComponent<PhotonView>();
        Player = GetComponent<CharacterController>();
        playerScale = transform.localScale;

        if (!Camera)
        {
            Camera = Camera.main;
        }

        PlayerCamera = Camera.transform;
        MouseLook.Init(PlayerTransform, PlayerCamera);
    }

    private void Update()
    {
        // Update player states
        //Check to make sure you are not moving other players
        if(!photonView.IsMine && PhotonNetwork.IsConnected == true){
            return;
        }

        MoveInput = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        crouching = Input.GetButton("Crouch");
        MouseLook.UpdateCursorLock();
        QueueJump();

        if (Player.isGrounded)
        {
            GroundMove();
        }
        else
        {
            AirMove();
        }

        if (Input.GetButtonDown("Crouch"))
        {
            StartCrouch();
        }

        if (Input.GetButtonUp("Crouch"))
        {
            StopCrouch();
        }

        var lastHeight = Player.height;
        Player.height = Mathf.Lerp(Player.height, currentHeightValue, 20 * Time.deltaTime);
        transform.position += new Vector3(0 , (Player.height - lastHeight) / 2, 0);
        MouseLook.LookRotation(PlayerTransform, PlayerCamera);
        Player.Move(PlayerVelocity * Time.deltaTime);
    }

    private void QueueJump()
    {
        // Handles jumping. If we want to turn on auto bunny hopping, that is also settled here
        if (AutoBunnyHop)
        {
            IsJumpQueued = Input.GetButton("Jump");
            return;
        }

        if (Input.GetButtonDown("Jump") && !IsJumpQueued)
        {
            IsJumpQueued = true;
        }

        if (Input.GetButtonUp("Jump"))
        {
            IsJumpQueued = false;
        }
    }

    private void AirMove()
    {
        float accel;

        // Wishdir is used to update player's velocity on the horizontal plane
        var wishdir = new Vector3(MoveInput.x, 0, MoveInput.z);
        wishdir = PlayerTransform.TransformDirection(wishdir);

        float wishspeed = wishdir.magnitude;
        wishspeed *= AirSettings.MaxSpeed;

        // Finds the direction in which we are going to move in the air
        wishdir.Normalize();
        MoveDirectionNormal = wishdir;

        float wishspeed2 = wishspeed;
        // Quake 3 model for "air strafe" mechanics.
        if (Vector3.Dot(PlayerVelocity, wishdir) < 0)
        {
            accel = AirSettings.Deceleration;
        }
        else
        {
            accel = AirSettings.Acceleration;
        }

        // If we are not holding forward, but we are moving to the side, we are able to "air strafe" using the wishspeed
        if (MoveInput.z == 0 && MoveInput.x != 0)
        {
            if (wishspeed > StrafeSettings.MaxSpeed)
            {
                wishspeed = StrafeSettings.MaxSpeed;
            }

            accel = StrafeSettings.Acceleration;
        }

        Accelerate(wishdir, wishspeed, accel);
        if (PlayerAirControl > 0)
        {
            AirControl(wishdir, wishspeed2);
        }

        // Apply gravity
        PlayerVelocity.y -= Gravity * Time.deltaTime;
    }

    // This allows players to move around corners while in midair much faster.
    private void AirControl(Vector3 targetDir, float targetSpeed)
    {
        // Ensures that we are doing "air movement" only while we have speed
        if (Mathf.Abs(MoveInput.z) < 0.001 || Mathf.Abs(targetSpeed) < 0.001)
        {
            return;
        }

        // Initialize speed
        float zSpeed = PlayerVelocity.y;
        PlayerVelocity.y = 0;
        float speed = PlayerVelocity.magnitude;
        PlayerVelocity.Normalize();

        float dot = Vector3.Dot(PlayerVelocity, targetDir);
        float k = 32;
        k *= PlayerAirControl * dot * dot * Time.deltaTime;

        // Slows down while changing our direction
        if (dot > 0)
        {
            PlayerVelocity.x *= speed + targetDir.x * k;
            PlayerVelocity.y *= speed + targetDir.y * k;
            PlayerVelocity.z *= speed + targetDir.z * k;

            PlayerVelocity.Normalize();
            MoveDirectionNormal = PlayerVelocity;
        }

        PlayerVelocity.x *= speed;
        PlayerVelocity.y = zSpeed;
        PlayerVelocity.z *= speed;
    }

    private void GroundMove()
    {
        // Ensures that we are only applying friction when on the ground
        if (!IsJumpQueued)
        {
            ApplyFriction(1.0f);
        }
        else
        {
            ApplyFriction(0);
        }

        // Direction in which the player moves
        var wishdir = new Vector3(MoveInput.x, 0, MoveInput.z);
        wishdir = PlayerTransform.TransformDirection(wishdir);
        wishdir.Normalize();
        MoveDirectionNormal = wishdir;

        var wishspeed = wishdir.magnitude;
        wishspeed *= GroundSettings.MaxSpeed;

        Accelerate(wishdir, wishspeed, GroundSettings.Acceleration);

        // Apply gravity
        PlayerVelocity.y = -Gravity * Time.deltaTime;

        // Jump
        if (IsJumpQueued)
        {
            PlayerVelocity.y = JumpForce;
            IsJumpQueued = false;
        }
    }

    private void ApplyFriction(float mult)
    {
        Vector3 vector = PlayerVelocity;
        vector.y = 0;
        float speed = vector.magnitude;
        float drop = 0;

        // Apply grounded friction
        if (Player.isGrounded)
        {
            float control = speed < GroundSettings.Deceleration ? GroundSettings.Deceleration : speed;
            drop = control * Friction * Time.deltaTime * mult;
        }

        float newSpeed = speed - drop;
        PlayerFriction = newSpeed;
        if (newSpeed < 0)
        {
            newSpeed = 0;
        }

        if (newSpeed > 0)
        {
            newSpeed /= speed;
        }

        PlayerVelocity.x *= newSpeed;
        PlayerVelocity.z *= newSpeed;
    }

    // Calculates acceleration based off of our speed and direction
    private void Accelerate(Vector3 targetDir, float targetSpeed, float acceleration)
    {
        float currentSpeed = Vector3.Dot(PlayerVelocity, targetDir);
        float addSpeed = targetSpeed - currentSpeed;
        if (addSpeed <= 0)
        {
            return;
        }

        float accelSpeed = acceleration * Time.deltaTime * targetSpeed;
        if (accelSpeed > addSpeed)
        {
            accelSpeed = addSpeed;
        }

        PlayerVelocity.x += accelSpeed * targetDir.x;
        PlayerVelocity.z += accelSpeed * targetDir.z;
    }

    private void StartCrouch()
    {
        // Scale player down to crouch size
        // TODO (if time permits): Animation to match crouch instead of "squishing" the player downwards to simulate a crouch
        //Player.height = crouchValue;
        //Player.center = new Vector3(Player.center.x, crouchCenterValue, Player.center.z);
        //transform.localScale = crouchScale;
        //transform.position = new Vector3(transform.position.x, transform.position.y - 10f, transform.position.z);
        currentHeightValue = crouchValue;
        // Crouch jumping
        if (!Player.isGrounded)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y + 5f, transform.position.z);
        }
    }

    private void StopCrouch()
    {
        // Re-scale player back to normal standing size
        //Player.height = standValue;
        //Player.center = new Vector3(Player.center.x, 0, Player.center.z);
        //transform.localScale = playerScale;
        //transform.position = new Vector3(transform.position.x, transform.position.y + crouchValue, transform.position.z);
        currentHeightValue = standValue;
    }
}
