using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Rolls a drop table and spawns the resulting pickups.
    /// </summary>
    public static class DropRoller
    {
        /// <summary>Rolls every entry once and spawns what drops around a point.</summary>
        public static void Roll(DropEntry[] drops, Vector2 origin, Material material)
        {
            for (int i = 0; i < drops.Length; i++)
            {
                DropEntry entry = drops[i];
                if (entry.item == null || Random.value > entry.chance) continue;

                int amount = Random.Range(entry.minAmount, entry.maxAmount + 1);
                if (entry.item.dropAsSingleStack)
                {
                    ItemSpawner.Spawn(entry.item, amount, origin, material);
                    continue;
                }

                for (int n = 0; n < amount; n++) ItemSpawner.Spawn(entry.item, 1, origin, material);
            }
        }
    }
}
