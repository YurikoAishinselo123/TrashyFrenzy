using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("Circle Cast")]
    [SerializeField] float range = 0.5f;
    [SerializeField] float radius = 1f;
    [SerializeField] LayerMask interactionLayer;
    [SerializeField] Transform origin;

    PlayerInventory inventory;

    void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.CircleCast(
            origin.position,
            radius,
            origin.right,
            range,
            interactionLayer
        );

        if (!hit)
            return;

        // ---------- AUTO COLLECT ----------
        if (hit.collider.TryGetComponent(out IAutoInteractable auto))
        {
            auto.AutoInteract(inventory);
            return;
        }

        // ---------- MANUAL INTERACT ----------
        if (!Input.GetKeyDown(KeyCode.E))
            return;

        if (hit.collider.TryGetComponent(out IManualInteractable manual))
        {
            manual.Interact(inventory);
        }
    }

    void OnDrawGizmosSelected()
    {
        if (origin == null)
            return;

        Gizmos.color = Color.cyan;

        Vector3 start = origin.position;
        Vector3 end = start + origin.right * range;

        Gizmos.DrawWireSphere(start, radius);
        Gizmos.DrawWireSphere(end, radius);
        Gizmos.DrawLine(start, end);
    }
}
