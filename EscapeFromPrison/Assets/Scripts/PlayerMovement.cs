using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(PlayerCollisionDetections))]
public class PlayerMovement : MonoBehaviour {

    #region vars

    // Requirements
    [SerializeField] private Transform orientation;
    private PlayerInput inputs;
    private PlayerCollisionDetections col;
    private PlayerFriction friction;

    [HideInInspector] public Rigidbody rb;

    // Movement
    [Header("Movement")]
    public float moveSpeed = 4500;
    public float maxSpeed = 20;

    // Crouch & Slide
    [Header("CrouchAndSlide")]
    public float slideForce = 400;
    private Vector3 crouchScale = new Vector3(1, 0.5f, 1);
    private Vector3 playerScale;

    // Jumping
    [Header("Jump")]
    public float jumpForce = 550f;
    private bool readyToJump = true;
    private float jumpCooldown = 0.25f;
    
    // Sliding
    private Vector3 normalVector = Vector3.up;
    private Vector3 wallNormalVector;

    #endregion

    void Awake() {
        rb = GetComponent<Rigidbody>();
        inputs = GetComponent<PlayerInput>();
        col = GetComponent<PlayerCollisionDetections>();
        friction = GetComponent<PlayerFriction>();
    }
    
    void Start() {
        playerScale =  transform.localScale;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void FixedUpdate() => Movement();

    private void Update() {
        //Crouching
        if (inputs.crouchDown)
            StartCrouch();
        if (inputs.crouchUp)
            StopCrouch();
    }

    private void Movement()
    {
        //Extra gravity
        rb.AddForce(Vector3.down * Time.deltaTime * 10);

        //Find actual velocity relative to where player is looking
        Vector2 mag = friction.FindVelRelativeToLook();
        float xMag = mag.x, yMag = mag.y;

        //Counteract sliding and sloppy movement
        friction.CounterMovement(inputs.x, inputs.y, mag);

        //If holding jump && ready to jump, then jump
        if (readyToJump && inputs.jump) Jump();

        //Set max speed
        float maxSpeed = this.maxSpeed;

        //If sliding down a ramp, add force down so player stays grounded and also builds speed
        if (inputs.crouch && col.grounded && readyToJump)
        {
            rb.AddForce(Vector3.down * Time.deltaTime * 3000);
            return;
        }

        //If speed is larger than maxspeed, cancel out the input so you don't go over max speed
        if (inputs.x > 0 && xMag > maxSpeed) inputs.x = 0;
        if (inputs.x < 0 && xMag < -maxSpeed) inputs.x = 0;
        if (inputs.y > 0 && yMag > maxSpeed) inputs.y = 0;
        if (inputs.y < 0 && yMag < -maxSpeed) inputs.y = 0;

        //Some multipliers
        float multiplier = 1f, multiplierV = 1f;

        // Movement in air
        if (!col.grounded)
        {
            multiplier = 0.5f;
            multiplierV = 0.5f;
        }

        // Movement while sliding
        if (col.grounded && inputs.crouch) multiplierV = 0f;

        //Apply forces to move player
        rb.AddForce(orientation.transform.forward * inputs.y * moveSpeed * Time.deltaTime * multiplier * multiplierV);
        rb.AddForce(orientation.transform.right * inputs.x * moveSpeed * Time.deltaTime * multiplier);
    }

    private void StartCrouch() {
        transform.localScale = crouchScale;
        transform.position = new Vector3(transform.position.x, transform.position.y - 0.5f, transform.position.z);
        if (rb.velocity.magnitude > 0.5f) {
            if (col.grounded) {
                rb.AddForce(orientation.transform.forward * slideForce);
            }
        }
    }

    private void StopCrouch() {
        transform.localScale = playerScale;
        transform.position = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);
    }

    private void Jump() {
        if (col.grounded && readyToJump) {
            readyToJump = false;

            //Add jump forces
            rb.AddForce(Vector2.up * jumpForce * 1.5f);
            rb.AddForce(normalVector * jumpForce * 0.5f);
            
            //If jumping while falling, reset y velocity.
            Vector3 vel = rb.velocity;
            if (rb.velocity.y < 0.5f)
                rb.velocity = new Vector3(vel.x, 0, vel.z);
            else if (rb.velocity.y > 0) 
                rb.velocity = new Vector3(vel.x, vel.y / 2, vel.z);
            
            Invoke(nameof(ResetJump), jumpCooldown);
        }
    }
    
    private void ResetJump() => readyToJump = true;
}
