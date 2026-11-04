using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// The gear a character is wearing. Equipping moves a piece out of the inventory and into a slot;
    /// unequipping moves it back. The piece keeps its level and nature while worn.
    /// Does not compute stats or display anything.
    /// </summary>
    [RequireComponent(typeof(Inventory))]
    public class Equipment : MonoBehaviour
    {
        private readonly Dictionary<EquipmentSlot, InventorySlot> worn = new Dictionary<EquipmentSlot, InventorySlot>();
        private Inventory inventory;

        /// <summary>Raised after any slot changes, or a worn piece changes (is upgraded or reforged).</summary>
        public event Action Changed;

        private void Awake()
        {
            inventory = GetComponent<Inventory>();
        }

        /// <summary>The item worn in a slot, or null.</summary>
        public ItemDefinition Get(EquipmentSlot slot)
        {
            InventorySlot piece = GetPiece(slot);
            return piece != null ? piece.Item : null;
        }

        /// <summary>The piece worn in a slot (with its level and nature), or null.</summary>
        public InventorySlot GetPiece(EquipmentSlot slot)
        {
            InventorySlot piece;
            return worn.TryGetValue(slot, out piece) ? piece : null;
        }

        /// <summary>True when this exact piece is being worn.</summary>
        public bool IsWorn(InventorySlot piece)
        {
            return worn.ContainsValue(piece);
        }

        /// <summary>
        /// Wears a piece from the inventory. The piece previously in the slot returns to the inventory.
        /// Returns false (and changes nothing) when the piece does not fit the slot or is not carried.
        /// </summary>
        public bool Equip(InventorySlot piece, EquipmentSlot slot)
        {
            if (piece == null || !inventory.Contains(piece) || !EquipmentSlots.Accepts(slot, piece.Item)) return false;

            InventorySlot previous = GetPiece(slot);
            inventory.Detach(piece);
            if (previous != null && !inventory.Insert(previous))
            {
                inventory.Insert(piece);
                return false;
            }

            worn[slot] = piece;
            RaiseChanged();
            return true;
        }

        /// <summary>Moves the piece in a slot back to the inventory. Returns false when the slot is empty or the inventory is full.</summary>
        public bool Unequip(EquipmentSlot slot)
        {
            InventorySlot piece = GetPiece(slot);
            if (piece == null || !inventory.Insert(piece)) return false;

            worn.Remove(slot);
            RaiseChanged();
            return true;
        }

        /// <summary>Announces that a worn piece changed (for example it was upgraded), so stats are recalculated.</summary>
        public void NotifyPieceChanged()
        {
            RaiseChanged();
        }

        private void RaiseChanged()
        {
            if (Changed != null) Changed();
        }
    }
}
