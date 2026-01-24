using System;

public static class InventoryEvents
{
    public static Action<ItemType, int> OnItemAdded;
    public static Action<ItemType, int> OnItemRemoved;
}
