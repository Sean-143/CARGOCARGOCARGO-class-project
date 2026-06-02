using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // InputAction declaration(s)
    public InputAction moveAction;
    public InputAction boostAction;
    public InputAction jumpAction;

    // Components of the GameObject
    private Rigidbody rb;
    private TruckExtensionsToPlayerCommunicator truckExtensionsCommunicator;

    // Adjustable parameters
    public float baseMoveSpeed = 10.0f;
    public float baseTurnSpeed = 10.0f;

    // Regarding normal movement functionality
    public Vector3 forward;
    private float moveSpeedScaler = 75.0f; // Universally scales movement speed of truck, for adjusting speed across the board

    // Regarding precision movement functionality
    private bool precisionMovementOn = false;
    private CharacterController characterController;
    private float groundedWindow = 0.2f;
    private Vector3 velocity;
    private float groundedTimer = 0.0f;

    // Regarding jump functionality
    private bool jumpOn = false;

    // Regarding boost functionality
    private bool boostOn = false; // Holds whether boost is currently active (as per Boost Extension)
    private bool boostActive = false; // Holds whether boost is currently occuring
    private float prevSpeed = 0.0f; // Holds base speed player was at immediately before boosting
    private float boostTimeElapsed = 0.0f; // Holds the duration of time boost has lasted for
    private bool boostTimeActive = false; // Holds whether boost timer is active
    private float boostCooldownTimeElapsed = 0.0f; // Holds the duration of time boost cooldown has run for
    private bool boostCooldownActive = false; // Holds whether boost cooldown is active

    // Start function
    void Start()
    {
        // Assigning InputActions
        this.moveAction = InputSystem.actions.FindAction("Move");
        this.boostAction = InputSystem.actions.FindAction("Interact");
        this.jumpAction = InputSystem.actions.FindAction("Jump");

        // Assigning components
        rb = this.GetComponent<Rigidbody>();
        truckExtensionsCommunicator = this.GetComponentInChildren<TruckExtensionsToPlayerCommunicator>();
        this.characterController = this.GetComponent<CharacterController>();

        // Normalizing directions
        forward = this.forward.normalized;

        // Assigning adjustable parameters to their actual stats in playerStats struct, held by TruckExtensionsToPlayerCommuicator
        truckExtensionsCommunicator.actualStats.moveSpeed = baseMoveSpeed;
        truckExtensionsCommunicator.actualStats.turnSpeed = baseTurnSpeed;
    }

    // Update function
    void FixedUpdate()
    {
        executeMovement();
        if (jumpOn && (jumpAction.ReadValue<float>() != 0.0f))
        {
            performJump();
        }

        if (boostOn && (boostAction.ReadValue<float>() != 0.0f) && !boostActive && !boostCooldownActive) { beginBoost(); }
        if (boostTimeActive)
        {
            boostTimeElapsed += Time.deltaTime;
            if (boostTimeElapsed >= truckExtensionsCommunicator.actualStats.boostDuration) { boostEnds(); }
        }
        if (boostCooldownActive)
        {
            boostCooldownTimeElapsed += Time.deltaTime;
            if (boostCooldownTimeElapsed >= truckExtensionsCommunicator.actualStats.boostCooldown) { boostCooldownActive = false; }
        }    
    }

    // Toggles precision movement on. There isn't a way to toggle it off, since this'll only ever be run once (if it's run at all) right before the level begins
    public void togglePrecisionMovement()
    { 
        precisionMovementOn = true;
        characterController.enabled = true; // Sets Character Controller to active (in the inspector, it's set to inactive by default)
    }

    // Toggles jumping on. There isn't a way to toggle it off, since this'll only ever be run once (if it's run at all) right before the level begins
    public void toggleJump()
    {
        jumpOn = true;
    }

    // Toggles boosting on. There isn't a way to toggle it off, since this'll only ever be run once (if it's run at all) right before the level begins
    public void toggleBoost()
    {
        boostOn = true;
    }

    // Executes basic movement of truck (temp, will likely be replaced as more factors are introduced that'll influence truck controlability)
    void executeMovement()
    {
        // Reads the player's input
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        // Applies the player input to their direction of movement
        Vector3 moveDirection = Vector3.zero; // Declares a base, zeroed-out moveDirection
        moveDirection += this.forward * moveInput.y;

        // Applies the player input to their direction of rotation
        Vector3 turnDirection = Vector3.zero;
        turnDirection.y = moveInput.x * truckExtensionsCommunicator.actualStats.turnSpeed;

        // Chooses between precision and normal movement depending on precisionMovementOn
        switch (precisionMovementOn)
        {
            case true:
                precisionMovement(moveDirection, turnDirection);
                break;
            case false:
                normalMovement(moveDirection, turnDirection);
                break;
        }
    }

    void normalMovement(Vector3 moveDir, Vector3 turnDir)
    {
        // Movement and rotation actually applied to player
        this.rb.AddForce(moveDir * truckExtensionsCommunicator.actualStats.moveSpeed * moveSpeedScaler * Time.deltaTime, ForceMode.Force);
        this.transform.Rotate(turnDir, Space.Self);

        // Changes forward to whatever current forward is
        this.forward = transform.forward;
    }

    void precisionMovement(Vector3 moveDir, Vector3 turnDir)
    {
        // Calculates the period of time that the character is grounded
        bool grounded = this.characterController.isGrounded;
        if (grounded)
        {
            this.groundedTimer = this.groundedWindow;
        }
        if (this.groundedTimer > 0.0f)
        {
            this.groundedTimer -= Time.deltaTime;
        }
        else
        {
            this.groundedTimer = 0.0f;
            if (grounded && this.velocity.y < 0)
            {
                this.velocity.y = 0.0f;
            }
        }

        // Handling gravity
        if (grounded && this.velocity.y < 0)
        {
            this.velocity.y = 0.0f;
        }
        this.velocity += Physics.gravity * Time.deltaTime;

        // Handling movement and turning
        moveDir *= truckExtensionsCommunicator.actualStats.moveSpeed;
        moveDir.y = this.velocity.y;
        moveDir *= Time.deltaTime;

        this.characterController.Move(moveDir);
        this.transform.Rotate(turnDir, Space.Self);
    }

    private void beginBoost()
    {
        // Inidicates that a boost is currently in-progress
        boostActive = true;

        // Sets up timer functionality
        boostTimeElapsed = 0.0f;
        boostTimeActive = true; // Flag set to true will initiate timer counting in Update() function

        // Stores current base speed, before switching to boostSpeed
        prevSpeed = truckExtensionsCommunicator.actualStats.moveSpeed;
        truckExtensionsCommunicator.actualStats.moveSpeed = truckExtensionsCommunicator.actualStats.boostSpeed;
    }

    private void boostEnds()
    {
        // Indicates that boost has concluded
        boostActive = false;

        // Blocks timer functionality
        boostTimeActive = false; // Flag set to false will halt timer counting in Update() function

        // Sets moveSpeed back to what its base value
        truckExtensionsCommunicator.actualStats.moveSpeed = prevSpeed;
    }

    private void performJump()
    {
        this.rb.AddForce(Vector3.up * truckExtensionsCommunicator.actualStats.jumpIntensity, ForceMode.Impulse);
    }
}
