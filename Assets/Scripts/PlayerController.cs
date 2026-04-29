using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // InputAction declaration(s)
    public InputAction moveAction;

    // Adjustable parameters
    public float moveSpeed = 10.0f;
    public float turnSpeed = 10.0f;

    // Regarding movement functionality
    public Vector3 forward;

    // Start function
    void Start()
    {
        // Assigning InputActions
        this.moveAction = InputSystem.actions.FindAction("Move");

        // Normalizing directions
        forward = this.forward.normalized;
    }

    // Update function
    void Update()
    {
        executeMovement();
    }

    // Executes basic movement of truck (temp, will likely be replaced as more factors are introduced that'll influence truck controlability)
    void executeMovement()
    {
        // Reads the player's input
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        // Applies the player input to their direction of movement
        Vector3 moveDirection = Vector3.zero;
        moveDirection += this.forward * moveInput.y;
        
        // Applies the player input to their direction of rotation
        Vector3 turnDirection = Vector3.zero;
        turnDirection.y = moveInput.x * turnSpeed;

        // Movement and rotation actually applied to player
        this.transform.position += moveDirection * moveSpeed * Time.deltaTime;
        this.transform.Rotate(turnDirection, Space.Self);

        // Changes forward to whatever current forward is
        this.forward = transform.forward;
    }
}
