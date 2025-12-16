using UnityEngine;

public class MobilePlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 13f;
    public float acceleration = 30f;
    public float drag = 10f;

    [Header("Rotation")]
    public float rotationSpeed = 10f;

    [Header("Input")]
    public VirtualJoystick joystick;

    Vector2 velocity;

    void Update()
    {
        ApplyJoystickForce();
        ApplyDrag();
        Move();
        RotateToVelocity();
    }

    void ApplyJoystickForce()
    {
        Vector2 input = joystick.InputVector;

        float inputMag = input.magnitude;
        if (inputMag < 0.01f)
            return;

        Vector2 force = input.normalized * acceleration * inputMag;
        velocity += force * Time.deltaTime;

        velocity = Vector2.ClampMagnitude(velocity, maxSpeed);
    }

    void ApplyDrag()
    {
        float dragStrength = joystick.InputVector.magnitude < 0.1f
            ? drag
            : drag * 0.3f;

        velocity = Vector2.MoveTowards(
            velocity,
            Vector2.zero,
            dragStrength * Time.deltaTime
        );

        if (velocity.sqrMagnitude < 0.0005f)
            velocity = Vector2.zero;
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
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0, 0, angle),
            rotationSpeed * Time.deltaTime
        );
    }
}
