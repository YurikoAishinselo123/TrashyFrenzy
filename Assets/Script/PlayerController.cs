using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 13f;
    public float acceleration = 30f;
    public float drag = 10f;

    [Header("Rotation")]
    public float rotationSpeed = 10f;

    [Header("Input")]
    public float mouseSensitivity = 1.2f;

    private PlayerInputActions input;
    private Vector2 velocity;
    private Vector2 mouseDelta;
    private bool isPressing;

    void Awake()
    {
        input = new PlayerInputActions();

        input.Player.Delta.performed += ctx => mouseDelta = ctx.ReadValue<Vector2>();
        input.Player.Delta.canceled += _ => mouseDelta = Vector2.zero;

        input.Player.Click.performed += _ => isPressing = true;
        input.Player.Click.canceled += _ => isPressing = false;
    }

    void OnEnable() => input.Enable();
    void OnDisable() => input.Disable();

    void Update()
    {
        bool thrusting = isPressing;

        if (thrusting)
            ApplyMouseDeltaForce();

        ApplyDrag();
        Move();
        RotateToVelocity();
    }

    void ApplyMouseDeltaForce()
    {
        if (mouseDelta.sqrMagnitude < 0.01f)
            return;

        Vector2 inputForce = mouseDelta * mouseSensitivity;

        velocity += inputForce * acceleration * Time.deltaTime;

        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
    }

    void ApplyDrag()
    {
        float dragStrength = mouseDelta.magnitude < 0.1f ? drag : drag * 0.05f;
        velocity = Vector2.MoveTowards(
            velocity,
            Vector2.zero,
            dragStrength * Time.deltaTime
        );
    }

    void Move()
    {
        transform.position += (Vector3)(velocity * Time.deltaTime);
    }

    void RotateToVelocity()
    {
        if (velocity.sqrMagnitude < 0.01f)
            return;

        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle);

        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            targetRotation,
            rotationSpeed * Time.deltaTime
        );
    }
}