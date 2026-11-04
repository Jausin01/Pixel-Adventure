using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the merchant in the town and the screen where items are bought and sold.
    /// Buy lines are on the left, sell lines on the right. Needs the items to exist already.
    /// </summary>
    public static class MerchantBuilder
    {
        private static readonly Vector2 MerchantPosition = TownLayout.Merchant;
        private static readonly Vector2 RowSize = new Vector2(550f, 92f);

        // Items for sale and their price in the currency. Gold ingots let the player pay the skill teacher with money.
        private static readonly string[] BuyIds = { "gold_ingot", "apple", "orange", "health_potion", "energy_potion" };
        private static readonly int[] BuyPrices = { 60, 6, 6, 25, 25 };

        // Items the merchant buys; the price is the item's own sell value.
        private static readonly string[] SellIds = { "coal", "wood", "silver_ingot", "amber", "amethyst" };

        /// <summary>Builds the merchant and its screen. Returns the screen so the caller can order it on top.</summary>
        public static InventoryPanel Build(Material material, Transform canvas, GameObject player)
        {
            Merchant merchant = player.AddComponent<Merchant>();
            merchant.currency = ItemAssetFactory.Load(ProjectConstants.CurrencyId);

            InventoryPanel screen = BuildScreen(canvas, player, merchant);
            BuildNpc(material, screen, merchant.currency);
            return screen;
        }

        private static void BuildNpc(Material material, InventoryPanel screen, ItemDefinition currency)
        {
            GameObject npc = new GameObject("Merchant");
            npc.transform.position = MerchantPosition;

            SpriteRenderer renderer = npc.AddComponent<SpriteRenderer>();
            renderer.sprite = SpriteLoader.Load("World/merchant");
            renderer.sharedMaterial = material;
            renderer.spriteSortPoint = SpriteSortPoint.Pivot;

            CircleCollider2D trigger = npc.AddComponent<CircleCollider2D>();
            trigger.isTrigger = true;
            trigger.radius = 1.1f;

            Interactable interactable = npc.AddComponent<Interactable>();
            interactable.screen = screen;
            interactable.icon = currency.icon;
        }

        private static InventoryPanel BuildScreen(Transform canvas, GameObject player, Merchant merchant)
        {
            GameObject root;
            InventoryPanel panel;
            Transform frame = ScreenWindowBuilder.Build(canvas, "Merchant Screen", "Merchant", out root, out panel);

            Vector2 topLeft = new Vector2(0f, 1f);
            Text buyTitle = UiFactory.CreateText("Buy Title", frame, "Buy", 32, TextAnchor.MiddleLeft, topLeft, new Vector2(24f, -72f), new Vector2(300f, 40f));
            Text sellTitle = UiFactory.CreateText("Sell Title", frame, "Sell", 32, TextAnchor.MiddleLeft, topLeft, new Vector2(24f + RowSize.x + 8f, -72f), new Vector2(300f, 40f));
            buyTitle.color = sellTitle.color = new Color(1f, 1f, 1f, 0.7f);

            for (int i = 0; i < BuyIds.Length; i++) BuildRow(frame, player, merchant, ItemAssetFactory.Load(BuyIds[i]), false, BuyPrices[i], 0, i);

            ShopRowView[] sellRows = new ShopRowView[SellIds.Length];
            for (int i = 0; i < SellIds.Length; i++)
            {
                sellRows[i] = BuildRow(frame, player, merchant, ItemAssetFactory.Load(SellIds[i]), true, 0, 1, i);
            }

            ShopSellList list = root.AddComponent<ShopSellList>();
            list.inventory = player.GetComponent<Inventory>();
            list.rows = sellRows;
            list.firstPosition = new Vector2(24f + RowSize.x + 8f, -116f);
            list.rowStep = RowSize.y + 6f;
            return panel;
        }

        private static ShopRowView BuildRow(Transform frame, GameObject player, Merchant merchant, ItemDefinition item, bool selling, int buyPrice, int column, int row)
        {
            Vector2 topLeft = new Vector2(0f, 1f);
            Vector2 position = new Vector2(24f + column * (RowSize.x + 8f), -116f - row * (RowSize.y + 6f));
            Image background = UiFactory.CreateImage(item.displayName + " Row", frame, null, topLeft, position, RowSize);
            background.color = new Color(0.22f, 0.16f, 0.13f, 1f);

            Image icon = UiFactory.CreateImage("Icon", background.transform, item.icon, new Vector2(0f, 0.5f), new Vector2(14f, 0f), new Vector2(64f, 64f));
            icon.preserveAspect = true;
            icon.raycastTarget = false;

            UiFactory.CreateText("Name", background.transform, item.displayName, 32, TextAnchor.MiddleLeft, topLeft, new Vector2(94f, -8f), new Vector2(300f, 42f));
            Text owned = UiFactory.CreateText("Owned", background.transform, string.Empty, 24, TextAnchor.MiddleLeft, topLeft, new Vector2(94f, -52f), new Vector2(300f, 32f));
            owned.color = new Color(1f, 1f, 1f, 0.7f);
            Text price = UiFactory.CreateText("Price", background.transform, string.Empty, 32, TextAnchor.MiddleRight, new Vector2(1f, 0.5f), new Vector2(-18f, 0f), new Vector2(200f, 50f));

            ShopRowView view = background.gameObject.AddComponent<ShopRowView>();
            view.item = item;
            view.merchant = merchant;
            view.inventory = player.GetComponent<Inventory>();
            view.ownedText = owned;
            view.priceText = price;
            view.selling = selling;
            view.buyPrice = buyPrice;

            ShopRowButton button = background.gameObject.AddComponent<ShopRowButton>();
            button.item = item;
            button.merchant = merchant;
            button.selling = selling;
            button.buyPrice = buyPrice;
            background.gameObject.AddComponent<PressScaleFeedback>();
            return view;
        }
    }
}
