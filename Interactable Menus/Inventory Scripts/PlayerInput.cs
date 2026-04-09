using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    public KeyCode inventoryKey = KeyCode.I;
    public KeyCode pickupKey = KeyCode.E;

    void Update()
    {
        // Toggle inventory
        if (Input.GetKeyDown(inventoryKey))
        {
            UI_Inventory.Instance.ToggleInventory();
        }

        // Pickup item
        if (Input.GetKeyDown(pickupKey))
        {
            //Debug.Log("E pressed");
            PlayerItemDetector.Instance.TryPickup();
        }
    }
}

