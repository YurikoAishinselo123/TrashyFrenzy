using UnityEngine;

public class CollectibleItem : MonoBehaviour, IAutoInteractable
{
    [SerializeField] int amount = 1;

    public void AutoInteract(PlayerInventory inventory)
    {
        inventory.AddItem(amount);

        if (SaveManager.Instance != null)
            SaveManager.Instance.SaveGame();

        Destroy(gameObject);
    }
}
