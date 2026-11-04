using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Buys and sells items for the currency. Does not display anything.
    /// </summary>
    [RequireComponent(typeof(Inventory))]
    public class Merchant : MonoBehaviour
    {
        [Tooltip("The item used as money.")]
        public ItemDefinition currency;

        private Inventory inventory;

        /// <summary>True when the player can pay the price.</summary>
        public bool CanAfford(int price)
        {
            return Items.Count(currency) >= price;
        }

        /// <summary>Buys one item. Returns false (and changes nothing) when the money or the room is missing.</summary>
        public bool TryBuy(ItemDefinition item, int price)
        {
            bool done = TryBuyCore(item, price);
            if (done) Sfx.Play("shop_buy"); else Sfx.Play("ui_error"); 
            return done;
        }

        private bool TryBuyCore(ItemDefinition item, int price)
        {
            if (!CanAfford(price)) return false;

            Items.Remove(currency, price);
            if (Items.Add(item, 1)) return true;

            Items.Add(currency, price); // no room for the item: give the money back
            return false;
        }

        /// <summary>Sells one item for its sell value. Returns false (and changes nothing) when it is not carried or has no value.</summary>
        public bool TrySell(ItemDefinition item)
        {
            bool done = TrySellCore(item);
            if (done) Sfx.Play("shop_sell"); 
            return done;
        }

        private bool TrySellCore(ItemDefinition item)
        {
            if (item.sellValue <= 0 || Items.Count(item) < 1) return false;

            Items.Remove(item, 1);
            if (Items.Add(currency, item.sellValue)) return true;

            Items.Add(item, 1); // the money does not fit in the inventory: undo the sale
            return false;
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
    }
}
