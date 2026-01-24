using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    public float maxSpeed = 13f;
    public float acceleration = 30f;
    public float drag = 10f;
    Vector2 playerVelocity;
    Vector2 externalForce;

    [Header("Rotation")]
    public float rotationSpeed = 10f;

    [Header("Input")]
    public bool requireClick = false;
    public float mouseSensitivity = 1.2f;
    PlayerInputActions input;
    Vector2 mouseDelta;
    bool isPressing;

    // New variables for fish current
    bool inCurrent = false;
    [SerializeField] float currentAccelerationMultiplier = 0.3f; // 30% of normal acceleration
    [SerializeField] float currentMaxSpeedMultiplier = 0.5f; // 50% of normal max speed

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
        ApplyExternalForces();   // ← Move this FIRST (sets inCurrent flag)
        HandleInput();           // ← Now this sees inCurrent = true
        ApplyDrag();

        // Use reduced max speed when in current
        float currentMaxSpeed = inCurrent ? maxSpeed * currentMaxSpeedMultiplier : maxSpeed;
        Vector2 finalVelocity = Vector2.ClampMagnitude(playerVelocity, currentMaxSpeed);

        transform.position += (Vector3)(finalVelocity * Time.deltaTime);
        RotateToVelocity(finalVelocity);

        // Reset current flag each frame (fish must reapply)
        inCurrent = false;
    }

    // ---------------- INPUT ----------------
    void HandleInput()
    {
        bool thrusting = !requireClick || isPressing;
        if (!thrusting || mouseDelta.sqrMagnitude < 0.01f)
            return;

        // Use reduced acceleration when in current
        float currentAccel = inCurrent ? acceleration * currentAccelerationMultiplier : acceleration;

        Vector2 inputForce = mouseDelta * mouseSensitivity;
        playerVelocity += inputForce * currentAccel * Time.deltaTime;
        playerVelocity = Vector2.ClampMagnitude(playerVelocity, maxSpeed);
    }

    // ---------------- EXTERNAL FORCES ----------------
    void ApplyExternalForces()
    {
        playerVelocity += externalForce;
        playerVelocity = Vector2.ClampMagnitude(playerVelocity, maxSpeed);

        // Reset external force each frame (must be reapplied continuously)
        externalForce = Vector2.zero;
    }

    public void ApplyExternalForce(Vector2 force)
    {
        externalForce += force;
        inCurrent = true; // Mark that player is in current
    }

    // ---------------- DRAG ----------------
    void ApplyDrag()
    {
        playerVelocity = Vector2.MoveTowards(
            playerVelocity,
            Vector2.zero,
            drag * Time.deltaTime
        );
    }

    // ---------------- ROTATION ----------------
    void RotateToVelocity(Vector2 vel)
    {
        if (vel.sqrMagnitude < 0.01f)
            return;

        float angle = Mathf.Atan2(vel.y, vel.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(
            transform.rotation,
            Quaternion.Euler(0f, 0f, angle),
            rotationSpeed * Time.deltaTime
        );
    }


    void CollectGarbgae()
    {

    }
}