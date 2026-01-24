using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    [SerializeField] PlayerInventory inventory;
    [SerializeField] StorageZone storage;

    void Awake()
    {
        Instance = this;
    }

    public void SaveGame()
    {
        GameSaveData data = new GameSaveData
        {
            inventoryCount = inventory.Count,
            storedCount = storage.StoredAmount
        };

        PlayerPrefs.SetString("SAVE", JsonUtility.ToJson(data));
    }

    public void LoadGame()
    {
        if (!PlayerPrefs.HasKey("SAVE"))
            return;

        var data = JsonUtility.FromJson<GameSaveData>(
            PlayerPrefs.GetString("SAVE")
        );

        inventory.SetCount(data.inventoryCount);
        storage.SetStored(data.storedCount);
    }

    public void DeleteSave()
    {
        PlayerPrefs.DeleteKey("SAVE");
    }
}