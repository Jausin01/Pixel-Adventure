namespace MobileRpg
{
    /// <summary>
    /// Touch button (a row on the merchant's screen): buys or sells one of the item per touch.
    /// </summary>
    public class ShopRowButton : PointerDownAction
    {
        /// <inheritdoc/>
        protected override string ClickSound { get { return null; } }

        public ItemDefinition item;
        public Merchant merchant;
        public bool selling;
        public int buyPrice;

        /// <inheritdoc/>
        protected override void Execute()
        {
            if (selling) merchant.TrySell(item);
            else merchant.TryBuy(item, buyPrice);
        }
    }
}
