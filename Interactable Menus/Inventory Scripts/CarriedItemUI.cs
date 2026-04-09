using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarriedItemUI : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI countText;

    public void SetItem(ItemData item, int count)
    {
        icon.sprite = item.icon;
        countText.text = count > 1 ? count.ToString() : "";
    }
}
