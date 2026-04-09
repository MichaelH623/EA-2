using UnityEngine;
using UnityEngine.EventSystems;

public class UISlot : MonoBehaviour, IPointerClickHandler
{
    public int slotIndex;

    public void OnPointerClick(PointerEventData eventData)
    {
        // Route the click directly to the main brain
        Inventory.Instance.OnSlotClicked(slotIndex, eventData.button);
    }
}



