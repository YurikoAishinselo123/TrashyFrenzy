using TMPro;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] PlayerInventory inventory;
    [SerializeField] TextMeshProUGUI text;

    void OnEnable()
    {
        if (inventory == null)
            return;

        inventory.OnChanged += UpdateUI;
        UpdateUI(inventory.Count); // initial sync
    }

    void OnDisable()
    {
        if (inventory == null)
            return;

        inventory.OnChanged -= UpdateUI;
    }

    void UpdateUI(int amount)
    {
        text.text = $"Garbage : {amount}";
    }
}