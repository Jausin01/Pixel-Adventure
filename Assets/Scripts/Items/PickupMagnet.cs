using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Pulls a pickup toward the player's inventory once the player is close, and collects it on arrival.
    /// </summary>
    [RequireComponent(typeof(ItemPickup))]
    public class PickupMagnet : MonoBehaviour
    {
        [Tooltip("Distance at which the pickup starts moving toward the player.")]
        public float attractRadius = 1.8f;

        [Tooltip("Speed at which the pickup flies to the player.")]
        public float speed = 7f;

        [Tooltip("Distance at which the pickup is collected.")]
        public float collectDistance = 0.3f;

        [Tooltip("Seconds after spawning before the magnet works (lets the pop-out finish).")]
        public float armDelay = 0.35f;

        private ItemPickup pickup;
        private Inventory inventory;
        private float age;

        private void Awake()
        {
            pickup = GetComponent<ItemPickup>();
        }

        private void Start()
        {
            PlayerMoveInput player = FindAnyObjectByType<PlayerMoveInput>();
            inventory = player != null ? player.GetComponent<Inventory>() : null; // not the stash, which is also an Inventory
        }

        private void Update()
        {
            age += Time.deltaTime;
            if (age < armDelay || inventory == null) return;

            Vector2 toPlayer = (Vector2)inventory.transform.position - (Vector2)transform.position;
            float distance = toPlayer.magnitude;
            if (distance > attractRadius) return;

            if (distance <= collectDistance)
            {
                pickup.Collect(inventory);
                return;
            }

            transform.position += (Vector3)(toPlayer / distance * speed * Time.deltaTime);
        }
    }
}
