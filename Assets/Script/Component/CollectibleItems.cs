using UnityEngine;

public class CollectibleItem : MonoBehaviour, IInteractable
{
    [SerializeField] int amount = 1;

    public void Interact(PlayerInventory inventory)
    {
        inventory.AddItem(amount);

        if (SaveManager.Instance != null)
            SaveManager.Instance.SaveGame();

        Destroy(gameObject);
    }
}