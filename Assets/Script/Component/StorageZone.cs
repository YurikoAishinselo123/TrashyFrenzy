using UnityEngine;
using System;

public class StorageZone : MonoBehaviour, IInteractable
{
    int storedAmount;
    public int StoredAmount => storedAmount;

    public event Action<int> OnStoredAmountChanged;

    public void Interact(PlayerInventory inventory)
    {
        int amount = inventory.TakeAll();
        if (amount <= 0)
            return;

        storedAmount += amount;
        OnStoredAmountChanged?.Invoke(storedAmount);
    }

    public void SetStored(int value)
    {
        storedAmount = Mathf.Max(0, value);
        OnStoredAmountChanged?.Invoke(storedAmount);
    }
}