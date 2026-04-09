using UnityEngine;

public class UI_Inventory : MonoBehaviour
{
    public static UI_Inventory Instance;

    public RectTransform inventoryPanel;
    public UISlot[] uiSlots;

    void Awake()
    {
        Instance = this;
    }

    public void ToggleInventory()
    {
        bool isActive = inventoryPanel.gameObject.activeSelf;
        inventoryPanel.gameObject.SetActive(!isActive);

        if (isActive) // inventory is closing
            Inventory.Instance.HandleInventoryClose();
    }

    public void Refresh()
    {
        var slots = Inventory.Instance.inventorySlots;

        // Clear and rebuild UI
        for (int i = 0; i < slots.Length; i++)
        {
            var slot = slots[i];

            // Get the transform directly from our UI array
            Transform targetTransform = uiSlots[i].transform;

            // 1. Clear old visuals
            foreach (Transform child in targetTransform)
                Destroy(child.gameObject);

            // 2. Spawn new visuals if the data says we have an item here
            if (!slot.IsEmpty)
            {
                if (Inventory.Instance.itemUIPrefab == null)
                {
                    Debug.LogError("CRITICAL: You forgot to assign the 'Item UI Prefab' in the Inventory script Inspector!");
                    return;
                }

                GameObject uiItemGO = Instantiate(Inventory.Instance.itemUIPrefab, targetTransform);
                UIItem uiItem = uiItemGO.GetComponent<UIItem>();

                if (uiItem == null)
                {
                    Debug.LogError("CRITICAL: The Prefab you assigned to 'Item UI Prefab' does NOT have the 'UIItem' script attached to it! Please open the prefab and attach the script.");
                    return; // Stop here so we don't crash the game!
                }

                uiItem.SetItem(slot.item, slot.count);
                uiItem.slotIndex = i;

                // Keep icon centered
                RectTransform rt = uiItemGO.GetComponent<RectTransform>();

                if (rt == null)
                {
                    Debug.LogError("CRITICAL: Your 'Item UI Prefab' is not a UI element! It needs to be a UI Image so it has a RectTransform.");
                    return;
                }

                rt.anchoredPosition = Vector2.zero;
            }
        }
    }
}