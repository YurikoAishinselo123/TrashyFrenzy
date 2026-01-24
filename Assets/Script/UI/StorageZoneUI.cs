using TMPro;
using UnityEngine;

public class StorageZoneUI : MonoBehaviour
{
    [SerializeField] StorageZone storage;
    [SerializeField] TextMeshProUGUI text;

    void OnEnable()
    {
        if (storage == null)
            return;

        storage.OnStoredAmountChanged += UpdateUI;
        UpdateUI(storage.StoredAmount); // initial sync
    }

    void OnDisable()
    {
        if (storage == null)
            return;

        storage.OnStoredAmountChanged -= UpdateUI;
    }

    void UpdateUI(int amount)
    {
        text.text = $"Stored : {amount}";
    }
}