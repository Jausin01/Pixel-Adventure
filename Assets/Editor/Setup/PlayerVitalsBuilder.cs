using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Gives the player health, energy, damage feedback, respawning and the ability to use items.
    /// </summary>
    public static class PlayerVitalsBuilder
    {
        /// <summary>Adds the vitals to the player.</summary>
        public static void Attach(GameObject player)
        {
            player.AddComponent<Health>().maxHealth = 100f;
            player.AddComponent<Energy>();
            player.AddComponent<HitFlash>().target = player.GetComponent<SpriteRenderer>();
            player.AddComponent<PlayerRespawn>();
            player.AddComponent<ItemUser>();
        }
    }
}
