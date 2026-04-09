using UnityEngine;

public enum ItemType
{
    Default,
    Helmet,
    Chestplate,
    Leggings,
    Shoes,
    Accessory,
    Consumable,
    Material
}

[CreateAssetMenu(menuName = "Items/ItemData")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public GameObject worldPrefab;
    public int maxStack = 1;
    public ItemType itemType;
}

