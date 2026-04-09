using UnityEngine;

public class WorldItem : MonoBehaviour
{
    public ItemData itemData;
    public int count;

    public void Initialize(ItemData data, int amount)
    {
        itemData = data;
        count = amount;
    }

    public void PickUp()
    {
        Inventory.Instance.AddItem(itemData, count);
        Destroy(gameObject);
    }
}

