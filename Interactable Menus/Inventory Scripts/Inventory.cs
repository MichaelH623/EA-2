using UnityEngine;
using UnityEngine.EventSystems;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    public InventorySlot[] inventorySlots = new InventorySlot[32];
    public GameObject itemUIPrefab;
    public Player player;

    [Header("Carried Item (Mouse)")]
    public ItemData carriedItem;
    public int carriedCount;
    public CarriedItemUI carriedItemUI;

    void Awake()
    {
        if (Instance == null) Instance = this;
    }

    void Update()
    {
        // Follow mouse cursor if carrying something
        if (carriedItem != null && carriedItemUI != null)
        {
            if (!carriedItemUI.gameObject.activeSelf)
            {
                carriedItemUI.gameObject.SetActive(true);
            }

            carriedItemUI.transform.position = Input.mousePosition;

            // Drop item if clicking outside the UI
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
            {
                if (!EventSystem.current.IsPointerOverGameObject())
                {
                    DropCarriedItem();
                }
            }
        }
        else if (carriedItemUI != null && carriedItemUI.gameObject.activeSelf)
        {
            carriedItemUI.gameObject.SetActive(false);
        }
    }

    public bool AddItem(ItemData item, int amount)
    {
        // Try stacking into existing slots
        foreach (var slot in inventorySlots)
        {
            if (!slot.IsEmpty && slot.item == item && slot.count < item.maxStack)
            {
                int space = item.maxStack - slot.count;
                int toAdd = Mathf.Min(space, amount);

                slot.count += toAdd;
                amount -= toAdd;

                if (amount <= 0)
                {
                    UI_Inventory.Instance.Refresh();
                    return true;
                }
            }
        }

        // Fill empty slots
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].IsEmpty)
            {
                int toPlace = Mathf.Min(item.maxStack, amount);

                inventorySlots[i].item = item;
                inventorySlots[i].count = toPlace;

                amount -= toPlace;

                if (amount <= 0)
                {
                    UI_Inventory.Instance.Refresh();
                    return true;
                }
            }
        }

        // Inventory full
        UI_Inventory.Instance.Refresh();
        return false;
    }


    public void OnSlotClicked(int slotIndex, PointerEventData.InputButton button)
    {
        InventorySlot slot = inventorySlots[slotIndex];

        if (carriedItem == null)
        {
            if (slot.IsEmpty) return;

            if (button == PointerEventData.InputButton.Left) PickUpStack(slot);
            else if (button == PointerEventData.InputButton.Right) PickUpHalf(slot);
        }
        else
        {
            if (button == PointerEventData.InputButton.Left)
            {
                if (slot.IsEmpty) PlaceWholeStack(slot);
                else if (slot.item == carriedItem) MergeWithSlot(slot);
                else SwapWithSlot(slot);
            }
            else if (button == PointerEventData.InputButton.Right)
            {
                PlaceOneItem(slot);
            }
        }

        RefreshUI();
    }

    #region Internal Logic

    private void PickUpStack(InventorySlot slot)
    {
        carriedItem = slot.item;
        carriedCount = slot.count;
        slot.Clear();

        carriedItemUI.SetItem(carriedItem, carriedCount);
        carriedItemUI.gameObject.SetActive(true);
        carriedItemUI.transform.position = Input.mousePosition;
    }

    private void PickUpHalf(InventorySlot slot)
    {
        int total = slot.count;
        carriedCount = Mathf.CeilToInt(total / 2f);
        int remain = total - carriedCount;

        carriedItem = slot.item;
        slot.count = remain;

        if (slot.count <= 0) slot.Clear();

        carriedItemUI.SetItem(carriedItem, carriedCount);
        carriedItemUI.gameObject.SetActive(true);
        carriedItemUI.transform.position = Input.mousePosition;
    }


    private void PlaceWholeStack(InventorySlot slot)
    {
        slot.item = carriedItem;
        slot.count = carriedCount;
        ClearHand();
    }

    private void PlaceOneItem(InventorySlot slot)
    {
        if (slot.IsEmpty)
        {
            slot.item = carriedItem;
            slot.count = 1;
            carriedCount--;
        }
        else if (slot.item == carriedItem && slot.count < slot.item.maxStack)
        {
            slot.count++;
            carriedCount--;
        }

        if (carriedCount <= 0) ClearHand();
    }

    private void MergeWithSlot(InventorySlot slot)
    {
        int space = slot.item.maxStack - slot.count;
        int toMove = Mathf.Min(space, carriedCount);

        slot.count += toMove;
        carriedCount -= toMove;

        if (carriedCount <= 0) ClearHand();
    }

    private void SwapWithSlot(InventorySlot slot)
    {
        ItemData tempItem = slot.item;
        int tempCount = slot.count;

        slot.item = carriedItem;
        slot.count = carriedCount;

        carriedItem = tempItem;
        carriedCount = tempCount;
    }

    private void ClearHand()
    {
        carriedItem = null;
        carriedCount = 0;
    }

    private void RefreshUI()
    {
        if (carriedItem != null)
            carriedItemUI.SetItem(carriedItem, carriedCount);

        UI_Inventory.Instance.Refresh();
    }

    #endregion

    public void HandleInventoryClose()
    {
        if (carriedItem == null) return;

        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].IsEmpty)
            {
                inventorySlots[i].item = carriedItem;
                inventorySlots[i].count = carriedCount;
                ClearHand();
                UI_Inventory.Instance.Refresh();
                return;
            }
        }
        DropCarriedItem();
    }

    private void DropCarriedItem()
    {
        // Start above the drop point
        Vector3 start = player.dropPoint.position + Vector3.up * 2f;

        // Raycast down to find the ground
        if (Physics.Raycast(start, Vector3.down, out RaycastHit hit, 5f))
        {
            // Spawn slightly above the ground
            Vector3 spawnPos = hit.point + Vector3.up * 0.25f;

            GameObject go = Instantiate(carriedItem.worldPrefab, spawnPos, Quaternion.identity);

            WorldItem wi = go.GetComponent<WorldItem>();
            wi.Initialize(carriedItem, carriedCount);

            Debug.Log("Drop Point Position: " + player.dropPoint.position);
            Debug.Log("Item Position: " + spawnPos);

            ClearHand();
            UI_Inventory.Instance.Refresh();
        }
    }
}