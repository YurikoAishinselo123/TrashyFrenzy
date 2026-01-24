using UnityEngine;
public class CollectibleItem : MonoBehaviour
{
    [SerializeField] ItemType itemType;

    public ItemType Type => itemType;

    public void Collect(PlayerInventory inventory)
    {
        inventory.AddItem(itemType);
        Destroy(gameObject);
    }
}
