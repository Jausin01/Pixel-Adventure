using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the money counter under the energy bar in the top-left corner.
    /// </summary>
    public static class CurrencyHudBuilder
    {
        private static readonly Vector2 Position = new Vector2(30f, -30f - 2f * 72f);

        /// <summary>Builds the counter. The player must already have an inventory.</summary>
        public static void Build(Transform parent, GameObject player)
        {
            ItemDefinition currency = ItemAssetFactory.Load(ProjectConstants.CurrencyId);
            Vector2 topLeft = new Vector2(0f, 1f);

            Image icon = UiFactory.CreateImage("Currency Icon", parent, currency.icon, topLeft, Position, new Vector2(48f, 48f));
            icon.raycastTarget = false;
            icon.preserveAspect = true;

            Text amount = UiFactory.CreateText("Currency Amount", parent, "0", 36, TextAnchor.MiddleLeft, topLeft, Position + new Vector2(60f, 0f), new Vector2(220f, 48f));
            amount.raycastTarget = false;

            CurrencyHud hud = icon.gameObject.AddComponent<CurrencyHud>();
            hud.inventory = player.GetComponent<Inventory>();
            hud.currency = currency;
            hud.amountText = amount;
        }
    }
}
