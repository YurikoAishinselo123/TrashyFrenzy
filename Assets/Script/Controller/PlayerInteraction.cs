using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] float range = 2f;
    [SerializeField] LayerMask interactionLayer;
    [SerializeField] Transform origin;

    PlayerInventory inventory;

    void Awake()
    {
        inventory = GetComponent<PlayerInventory>();
    }

    void Update()
    {
        if (!Input.GetKeyDown(KeyCode.E)) return;

        RaycastHit2D hit = Physics2D.Raycast(origin.position, origin.right, range, interactionLayer);
        if (!hit) return;
        Debug.Log("Raycast hit: " + hit.collider.name);

        if (hit.collider.TryGetComponent(out CollectibleItem item))
        {
            Debug.Log("collected");
            item.Collect(inventory);
            return;
        }

        if (hit.collider.TryGetComponent(out StorageZone storage))
        {
            storage.TryStore(inventory);
        }

    }
}
