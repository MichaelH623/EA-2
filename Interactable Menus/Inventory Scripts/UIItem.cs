using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{
    public ItemData itemData;
    public int slotIndex;
    public TextMeshProUGUI quantityText;

    private Image icon;

    void Awake()
    {
        icon = GetComponent<Image>();

        //IMPORTANT: This ensures clicks pass through the item picture and hit the slot behind it!
        icon.raycastTarget = false;
        if (quantityText != null) quantityText.raycastTarget = false;
    }

    public void SetItem(ItemData data, int count)
    {
        itemData = data;
        if (icon == null) icon = GetComponent<Image>();
        icon.sprite = data.icon;
        UpdateCount(count);
    }

    public void UpdateCount(int count)
    {
        quantityText.text = count > 1 ? count.ToString() : "";
    }
}