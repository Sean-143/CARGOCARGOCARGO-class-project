using UnityEngine;
using UnityEngine.InputSystem;

// A script created to test the functionality of another car

public class TestCarBehavior : MonoBehaviour
{
    //public Rigidbody rb;
    public WheelCollider frontRightWheel;
    public WheelCollider frontLeftWheel;
    public WheelCollider backRightWheel;
    public WheelCollider backLeftWheel;

    public float moveSpeed = 10.0f;
    public float turnSpeed = 2.0f;

    public InputAction moveAction;
    private Vector2 moveInput;

    private void Start()
    {
        this.moveAction = InputSystem.actions.FindAction("Move");
    }

    /*void Update()
    {
        moveInput = moveAction.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        float motorSim = moveInput.y * moveSpeed;
        float steerSim = moveInput.x * turnSpeed;

        frontRightWheel.motorTorque = motorSim;
        frontLeftWheel.motorTorque = motorSim;
        backRightWheel.motorTorque = motorSim;
        backLeftWheel.motorTorque = motorSim;
        frontRightWheel.steerAngle = steerSim;
        frontLeftWheel.steerAngle = steerSim;
    }*/
}
