using UnityEngine;

[RequireComponent(typeof(SchoolFishController))]
public class SchoolFishPush : MonoBehaviour
{
    [SerializeField] float pushForce = 0.5f;

    SchoolFishController fish;

    void Awake()
    {
        fish = GetComponent<SchoolFishController>();
    }

    void OnTriggerEnter2D(Collider2D other) // Changed from Stay to Enter
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null)
            return;

        Vector2 fishDirection = fish.GetVelocity().normalized;

        // Apply force on enter
        player.ApplyExternalForce(fishDirection * pushForce);
    }

    void OnTriggerStay2D(Collider2D other) // Keep this too for continuous push
    {
        if (!other.CompareTag("Player"))
            return;

        PlayerController player = other.GetComponent<PlayerController>();
        if (player == null)
            return;

        Vector2 fishDirection = fish.GetVelocity().normalized;

        // Continuous push while inside
        player.ApplyExternalForce(fishDirection * pushForce);
    }
}