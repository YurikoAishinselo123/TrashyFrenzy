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
    bool isPressing;


    // Mouse / Trackpad
    Vector2 mouseDelta;

    // Virtual Analog
    Vector2 analogInput;


    [Header("Input Mode")]
    [SerializeField] bool useMobileInput = false;


    // New variables for fish current
    bool inCurrent = false;
    [SerializeField] float currentAccelerationMultiplier = 0.3f; // 30% of normal acceleration
    [SerializeField] float currentMaxSpeedMultiplier = 0.5f; // 50% of normal max speed
    [SerializeField] SpriteRenderer spriteRenderer;

    void Awake()
    {
        input = new PlayerInputActions();

        // Mouse / Trackpad
        input.Player.Delta.performed += ctx => mouseDelta = ctx.ReadValue<Vector2>();
        input.Player.Delta.canceled += _ => mouseDelta = Vector2.zero;
        input.Player.Click.performed += _ => isPressing = true;
        input.Player.Click.canceled += _ => isPressing = false;

        // Virtual Analog
        input.Player.Move.performed += ctx => analogInput = ctx.ReadValue<Vector2>();
        input.Player.Move.canceled += _ => analogInput = Vector2.zero;
    }

    // void Start()
    // {
    //     useMobileInput = Application.isMobilePlatform;
    // }

    Vector2 GetMovementInput(out bool isActive)
    {
        if (useMobileInput)
        {
            float magnitude = analogInput.magnitude;
            isActive = magnitude > 0.15f; // dead zone
            return analogInput;
        }
        else
        {
            isActive = !requireClick || isPressing;
            return mouseDelta;
        }
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
        FaceLeftRight(finalVelocity);
        // Reset current flag each frame (fish must reapply)
        inCurrent = false;
    }

    // ---------------- INPUT ----------------
    void HandleInput()
    {
        Vector2 inputVector = GetMovementInput(out bool isActive);
        if (!isActive || inputVector.sqrMagnitude < 0.01f)
            return;

        float currentAccel = inCurrent
            ? acceleration * currentAccelerationMultiplier
            : acceleration;

        if (useMobileInput)
        {
            // ANALOG (direction + strength)
            Vector2 dir = inputVector.normalized;
            float strength = Mathf.Clamp01(inputVector.magnitude);

            playerVelocity += dir * currentAccel * strength * Time.deltaTime;
        }
        else
        {
            // MOUSE / TRACKPAD (delta impulse)
            playerVelocity += inputVector * mouseSensitivity * currentAccel * Time.deltaTime;
        }

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
    void FaceLeftRight(Vector2 vel)
    {
        if (Mathf.Abs(vel.x) < 0.05f)
            return;

        spriteRenderer.flipX = vel.x < 0f;
    }
}