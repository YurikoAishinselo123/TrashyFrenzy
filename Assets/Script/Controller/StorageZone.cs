using TMPro;
using UnityEngine;
using TMPro;


public class StorageZone : MonoBehaviour
{
    [SerializeField] ItemType acceptedType;
    [SerializeField] int requiredAmount = 1;

    [SerializeField] private TextMeshProUGUI storeGarbage;

    int storedAmount;

    public bool TryStore(PlayerInventory inventory)
    {
        if (!inventory.HasItem(acceptedType, requiredAmount))
            return false;

        inventory.RemoveItem(acceptedType, requiredAmount);
        storedAmount += requiredAmount;

        storeGarbage.text = "Store : " + storedAmount.ToString();
        Debug.Log($"Stored {requiredAmount} {acceptedType}");
        return true;
    }
}
