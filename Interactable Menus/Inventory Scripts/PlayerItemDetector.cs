using UnityEngine;

public class PlayerItemDetector : MonoBehaviour
{
    [SerializeField] private float detectionRadius = 2f;
    [SerializeField] private LayerMask itemLayer;

    private WorldItem detectedItem;   // store the world object
    public static PlayerItemDetector Instance;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        DetectItem();
    }

    private void DetectItem()
    {
        // Use 3D physics instead of 2D
        Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, itemLayer);

        if (hits.Length > 0)
        {
            Collider hit = hits[0];

            //Debug.Log("Detected: " + hit.name);

            detectedItem = hit.GetComponent<WorldItem>();

            if (detectedItem == null)
            {
                Debug.LogWarning("Hit object has no PickupItem component: " + hit.name);
            }
        }
        else
        {
            detectedItem = null;
        }
    }

    public void TryPickup()
    {
        if (detectedItem != null)
        {
            // AddItem returns true if it worked, false if inventory is full
            bool addedToInventory = Inventory.Instance.AddItem(detectedItem.itemData, detectedItem.count);

            if (addedToInventory)
            {
                Destroy(detectedItem.gameObject);
                detectedItem = null;
            }
            else
            {
                Debug.Log("Inventory is full!");
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}




