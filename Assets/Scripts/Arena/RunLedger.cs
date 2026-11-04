using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Keeps track of what the player collected during an arena run, so a share of it can be taken away on death.
    /// Only items gained since <see cref="Begin"/> count; whatever the player already owned is never touched.
    /// </summary>
    [RequireComponent(typeof(Inventory))]
    public class RunLedger : MonoBehaviour
    {
        private readonly Dictionary<ItemDefinition, int> known = new Dictionary<ItemDefinition, int>();
        private readonly Dictionary<ItemDefinition, int> gained = new Dictionary<ItemDefinition, int>();
        private Inventory inventory;
        private bool tracking;

        /// <summary>Raised when the amounts gained change, or counting starts or stops.</summary>
        public event Action Changed;

        /// <summary>True while a run is being tracked.</summary>
        public bool Active { get { return tracking; } }

        /// <summary>How many of an item were collected during the current run.</summary>
        public int GainedCount(ItemDefinition item)
        {
            int count;
            return tracking && gained.TryGetValue(item, out count) ? count : 0;
        }

        private void Awake()
        {
            inventory = GetComponent<Inventory>();
        }

        private void OnEnable()
        {
            if (inventory == null) inventory = GetComponent<Inventory>();
            inventory.Changed += OnInventoryChanged;
        }

        private void OnDisable()
        {
            inventory.Changed -= OnInventoryChanged;
        }

        /// <summary>Starts counting from what the player owns right now.</summary>
        public void Begin()
        {
            known.Clear();
            gained.Clear();

            for (int i = 0; i < inventory.Slots.Count; i++)
            {
                InventorySlot slot = inventory.Slots[i];
                int count;
                known.TryGetValue(slot.Item, out count);
                known[slot.Item] = count + slot.Count;
            }
            tracking = true;
            RaiseChanged();
        }

        /// <summary>Stops counting. Nothing is taken away.</summary>
        public void End()
        {
            tracking = false;
            RaiseChanged();
        }

        /// <summary>Stops counting and removes a share of what was gained, rounded down.</summary>
        /// <param name="fraction">Share to remove, from 0 to 1.</param>
        /// <returns>How many items were removed in total.</returns>
        public int ApplyPenalty(float fraction)
        {
            tracking = false;
            int totalLost = 0;

            foreach (KeyValuePair<ItemDefinition, int> entry in new Dictionary<ItemDefinition, int>(gained))
            {
                int lost = Mathf.FloorToInt(entry.Value * Mathf.Clamp01(fraction));
                lost = Mathf.Min(lost, inventory.Count(entry.Key));
                if (lost <= 0) continue;

                inventory.Remove(entry.Key, lost);
                totalLost += lost;
            }
            gained.Clear();
            RaiseChanged();
            return totalLost;
        }

        private void OnInventoryChanged(ItemDefinition item, int total)
        {
            if (!tracking) return;

            int previous;
            known.TryGetValue(item, out previous);
            known[item] = total;

            int soFar;
            gained.TryGetValue(item, out soFar);
            gained[item] = Mathf.Max(0, soFar + total - previous);
            RaiseChanged();
        }

        private void RaiseChanged()
        {
            if (Changed != null) Changed();
        }
    }
}
