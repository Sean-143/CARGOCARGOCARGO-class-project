using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class TestPlayerBehavior : MonoBehaviour
{
    private InputAction moveAction;

    public CharacterController characterController = null;
    public float moveSpeed = 15.0f;
    public float turnSpeed = 0.3f;
    public float groundedWindow = 0.2f;

    private Vector3 velocity;
    private float groundedTimer = 0.0f;

    private void Start()
    {
        this.moveAction = InputSystem.actions.FindAction("Move");
    }

    private void Update()
    {
        characterControllerUpdate();
    }

    // Basic character controller functionality, including some modifications
    private void characterControllerUpdate()
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

        // Handling movement
        Vector3 movement = Vector3.zero;
        Vector3 turning = Vector3.zero;
        Vector2 moveInput = moveAction.ReadValue<Vector2>();

        movement += moveInput.y * this.transform.forward;
        movement *= this.moveSpeed;
        movement.y = this.velocity.y;
        movement *= Time.deltaTime;

        turning.y += moveInput.x * turnSpeed;

        this.characterController.Move(movement);
        this.transform.Rotate(turning, Space.Self);
    }
}