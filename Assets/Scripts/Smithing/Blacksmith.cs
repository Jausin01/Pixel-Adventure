using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Upgrades and reforges gear for materials and money. Does not display anything.
    /// An upgrade raises the piece one level (+1 to +5). A reforge sends it back to +0, adds a permanent bonus and gives
    /// it a new random nature, which can be better or worse than the old one.
    /// </summary>
    [RequireComponent(typeof(Inventory), typeof(Equipment))]
    public class Blacksmith : MonoBehaviour
    {
        [Tooltip("The item used as money.")]
        public ItemDefinition currency;

        [Tooltip("A reforge costs this many upgrade steps, plus one for every earlier reforge of the piece.")]
        public int reforgeBaseSteps = 3;

        private Inventory inventory;
        private Equipment equipment;

        /// <summary>The price of the next upgrade, or null when the piece cannot be upgraded further.</summary>
        public SmithCost UpgradeCost(InventorySlot piece)
        {
            if (piece == null || piece.Gear == null || !piece.Gear.CanLevelUp) return null;
            return Scale(piece.Item.tier, piece.Gear.Level + 1);
        }

        /// <summary>The price of reforging the piece, or null when it cannot be reforged.</summary>
        public SmithCost ReforgeCost(InventorySlot piece)
        {
            if (piece == null || piece.Gear == null) return null;
            return Scale(piece.Item.tier, reforgeBaseSteps + piece.Gear.Reforges);
        }

        /// <summary>True when the player carries everything the cost asks for.</summary>
        public bool CanAfford(SmithCost cost)
        {
            if (cost == null) return false;

            for (int i = 0; i < cost.materials.Length; i++)
            {
                if (Items.Count(cost.materials[i].item) < cost.materials[i].amount) return false;
            }
            return Items.Count(currency) >= cost.shards;
        }

        /// <summary>Raises the piece one level. Returns false (and takes nothing) when it cannot.</summary>
        public bool TryUpgrade(InventorySlot piece)
        {
            bool done = TryUpgradeCore(piece);
            if (done) Sfx.Play("forge"); else Sfx.Play("ui_error"); 
            return done;
        }

        private bool TryUpgradeCore(InventorySlot piece)
        {
            SmithCost cost = UpgradeCost(piece);
            if (!CanAfford(cost)) return false;

            Pay(cost);
            piece.Gear.LevelUp();
            AnnounceChange(piece);
            return true;
        }

        /// <summary>Reforges the piece. Returns false (and takes nothing) when it cannot.</summary>
        public bool TryReforge(InventorySlot piece)
        {
            bool done = TryReforgeCore(piece);
            if (done) Sfx.Play("forge"); else Sfx.Play("ui_error"); 
            return done;
        }

        private bool TryReforgeCore(InventorySlot piece)
        {
            SmithCost cost = ReforgeCost(piece);
            if (!CanAfford(cost)) return false;

            Pay(cost);
            GearNatureTable table = piece.Item.natures;
            piece.Gear.Reforge(table != null ? table.Roll(piece.Item) : null);
            AnnounceChange(piece);
            return true;
        }

        private SmithCost Scale(ItemTier tier, int steps)
        {
            if (tier == null || tier.smithMaterials == null) return null;

            SmithCost cost = new SmithCost { materials = new ItemAmount[tier.smithMaterials.Length], shards = tier.smithShards * steps };
            for (int i = 0; i < cost.materials.Length; i++)
            {
                cost.materials[i] = new ItemAmount { item = tier.smithMaterials[i].item, amount = tier.smithMaterials[i].amount * steps };
            }
            return cost;
        }

        private void Pay(SmithCost cost)
        {
            for (int i = 0; i < cost.materials.Length; i++) Items.Remove(cost.materials[i].item, cost.materials[i].amount);
            if (cost.shards > 0) Items.Remove(currency, cost.shards);
        }

        // Lets the inventory screens and the stats notice that the piece is different now.
        private void AnnounceChange(InventorySlot piece)
        {
            if (Gear.IsWorn(piece)) Gear.NotifyPieceChanged();
            else Items.Touch(piece.Item);
        }

        // Resolved on first use: other components may ask before this object's Awake has run.
        private Inventory Items
        {
            get
            {
                if (inventory == null) inventory = GetComponent<Inventory>();
                return inventory;
            }
        }

        private Equipment Gear
        {
            get
            {
                if (equipment == null) equipment = GetComponent<Equipment>();
                return equipment;
            }
        }
    }
}
