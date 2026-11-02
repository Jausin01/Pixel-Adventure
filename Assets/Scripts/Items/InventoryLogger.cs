using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Debug helper: prints inventory changes to the Console until a real inventory UI exists.
    /// </summary>
    [RequireComponent(typeof(Inventory))]
    public class InventoryLogger : MonoBehaviour
    {
        private Inventory inventory;

        private void Awake()
        {
            inventory = GetComponent<Inventory>();
        }

        private void OnEnable()
        {
            inventory.Changed += OnChanged;
        }

        private void OnDisable()
        {
            inventory.Changed -= OnChanged;
        }

        private void OnChanged(ItemDefinition item, int total)
        {
            Debug.Log("Inventory: " + item.displayName + " x" + total);
        }
    }
}
