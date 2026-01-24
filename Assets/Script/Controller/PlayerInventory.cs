using UnityEngine;
using System.Collections.Generic;
using TMPro;


public class PlayerInventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerGarbage;

    Dictionary<ItemType, int> items = new Dictionary<ItemType, int>();


    public void AddItem(ItemType type)
    {
        items.TryGetValue(type, out int count);
        items[type] = count + 1;
        UpdateUI(type);
    }

    public bool HasItem(ItemType type, int amount = 1)
    {
        return items.TryGetValue(type, out int count) && count >= amount;
    }

    public void RemoveItem(ItemType type, int amount = 1)
    {
        if (!HasItem(type, amount)) return;

        items[type] -= amount;
        if (items[type] <= 0)
            items.Remove(type);
    }

    public int GetItemCount(ItemType type)
    {
        return items.TryGetValue(type, out int count) ? count : 0;
    }


    void UpdateUI(ItemType type)
    {
        if (type == ItemType.Garbage && playerGarbage != null)
        {
            int count = GetItemCount(ItemType.Garbage);
            playerGarbage.text = $"Garbage : {count}";
        }
    }
}
