using System;
using System.Collections.Generic;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// The state of the blacksmith's screen: every piece of gear the player owns (worn first), the page of the list that is
    /// shown and the selected piece. The list, details and buttons only read from here and report player input back.
    /// Must stay on an object that is always active.
    /// </summary>
    public class SmithBrowser : MonoBehaviour
    {
        /// <summary>Rows per page of the list.</summary>
        public const int PageSize = 5;

        public Inventory inventory;
        public Equipment equipment;

        private readonly List<InventorySlot> pieces = new List<InventorySlot>();
        private int page;

        /// <summary>Raised when the pieces, the page or the selection change.</summary>
        public event Action Changed;

        /// <summary>The selected piece, or null.</summary>
        public InventorySlot Selected { get; private set; }

        /// <summary>The shown page, from 0.</summary>
        public int Page { get { return page; } }

        /// <summary>Number of pages, at least 1.</summary>
        public int PageCount { get { return Mathf.Max(1, Mathf.CeilToInt(pieces.Count / (float)PageSize)); } }

        private void OnEnable()
        {
            inventory.Changed += OnInventoryChanged;
            equipment.Changed += Rebuild;
            Rebuild();
        }

        private void OnDisable()
        {
            inventory.Changed -= OnInventoryChanged;
            equipment.Changed -= Rebuild;
        }

        /// <summary>The piece shown in a row of the current page, or null when the row is empty.</summary>
        public InventorySlot PieceAt(int row)
        {
            int index = page * PageSize + row;
            return index < pieces.Count ? pieces[index] : null;
        }

        /// <summary>True when the piece is being worn.</summary>
        public bool IsWorn(InventorySlot piece)
        {
            return equipment.IsWorn(piece);
        }

        /// <summary>Selects a piece.</summary>
        public void Select(InventorySlot piece)
        {
            Selected = piece;
            RaiseChanged();
        }

        /// <summary>Moves to another page (+1 next, -1 previous), staying inside the list.</summary>
        public void ChangePage(int direction)
        {
            page = Mathf.Clamp(page + direction, 0, PageCount - 1);
            RaiseChanged();
        }

        private void OnInventoryChanged(ItemDefinition item, int total)
        {
            Rebuild();
        }

        private void Rebuild()
        {
            pieces.Clear();
            for (int i = 0; i < EquipmentSlots.All.Length; i++)
            {
                InventorySlot worn = equipment.GetPiece(EquipmentSlots.All[i]);
                if (worn != null) pieces.Add(worn);
            }

            for (int i = 0; i < inventory.Slots.Count; i++)
            {
                if (inventory.Slots[i].Gear != null) pieces.Add(inventory.Slots[i]);
            }

            if (Selected == null || !pieces.Contains(Selected)) Selected = pieces.Count > 0 ? pieces[0] : null;
            page = Mathf.Clamp(page, 0, PageCount - 1);
            RaiseChanged();
        }

        private void RaiseChanged()
        {
            if (Changed != null) Changed();
        }
    }
}
