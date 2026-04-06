using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("Interaction Settings")]
    public string ItemName;
    public float interactionRadius = 3f;

    public string GetItemName()
    {
        return ItemName;
    }

    // Check if a given position (player) is inside the radius
    public bool IsPlayerInRange(Transform player)
    {
        float dist = Vector3.Distance(player.position, transform.position);
        return dist <= interactionRadius;
    }

    // Draw the radius in the editor
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }

    public Transform playerTransform;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && IsPlayerInRange(playerTransform) && SelectionManager.Instance.onTarget &&  SelectionManager.Instance.selectedObject == gameObject)
        {
            if (!InventorySystem.Instance.isOpen && !CraftingSystem.Instance.isOpenCraft)
            {
                //if the inventory is NOT full
                if (!InventorySystem.Instance.CheckIfFull())
                {
                    InventorySystem.Instance.AddToInventory(ItemName);
                    Destroy(gameObject);
                }
                else
                {
                    Debug.Log("Inventory is full");
                }
            }
        }
    }
}

