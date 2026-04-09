using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemData item;
    public int count;

    public bool IsEmpty => item == null;

    public bool CanStack(ItemData newItem) => item == newItem && count < item.maxStack;

    public void Clear()
    {
        item = null;
        count = 0;
    }
}

