using UnityEngine;
using System;

public class PlayerInventory : MonoBehaviour
{
    int count;
    public int Count => count;

    public event Action<int> OnChanged;

    public void AddItem(int amount = 1)
    {
        count += amount;
        OnChanged?.Invoke(count);
    }

    public bool RemoveItem(int amount = 1)
    {
        if (count < amount)
            return false;

        count -= amount;
        OnChanged?.Invoke(count);
        return true;
    }

    // 🔑 BULK TRANSFER API
    public int TakeAll()
    {
        int amount = count;
        count = 0;
        OnChanged?.Invoke(count);
        return amount;
    }

    public void SetCount(int value)
    {
        count = Mathf.Max(0, value);
        OnChanged?.Invoke(count);
    }
}