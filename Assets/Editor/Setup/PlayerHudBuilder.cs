using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates the player's health and energy bars in the top-left corner of the screen.
    /// </summary>
    public static class PlayerHudBuilder
    {
        private const float Margin = 30f;
        private const float Scale = 8f; // canvas units per art pixel
        private const float RowSpacing = 72f;

        /// <summary>Builds both bars. The player must already have <see cref="Health"/> and <see cref="Energy"/>.</summary>
        public static void Build(Transform parent, GameObject player)
        {
            UiMeterBarBuilder.Build(parent, SpriteLoader.Load("Hud/heart"), SpriteLoader.Load("Hud/hp_empty"),
                SpriteLoader.Load("Hud/hp_full"), new Vector2(Margin, -Margin), Scale, player.GetComponent<Health>());

            UiMeterBarBuilder.Build(parent, SpriteLoader.Load("Hud/bolt"), SpriteLoader.Load("Hud/ap_empty"),
                SpriteLoader.Load("Hud/ap_full"), new Vector2(Margin, -Margin - RowSpacing), Scale, player.GetComponent<Energy>());
        }
    }
}
