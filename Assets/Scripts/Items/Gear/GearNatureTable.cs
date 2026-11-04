using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Every nature gear can be born with, and the dice that pick one.
    /// </summary>
    [CreateAssetMenu(menuName = "Mobile RPG/Gear Natures", fileName = "GearNatures")]
    public class GearNatureTable : ScriptableObject
    {
        public GearNature[] natures;

        /// <summary>The nature with this id, or null.</summary>
        public GearNature Find(string id)
        {
            for (int i = 0; natures != null && i < natures.Length; i++)
            {
                if (natures[i].id == id) return natures[i];
            }
            return null;
        }

        /// <summary>Picks a random nature for the item, ignoring natures that would change none of its stats.</summary>
        public GearNature Roll(ItemDefinition item)
        {
            if (natures == null) return null;

            float total = 0f;
            for (int i = 0; i < natures.Length; i++)
            {
                if (natures[i].FitsItem(item)) total += natures[i].weight;
            }

            float pick = Random.value * total;
            for (int i = 0; i < natures.Length; i++)
            {
                if (!natures[i].FitsItem(item)) continue;

                pick -= natures[i].weight;
                if (pick <= 0f) return natures[i];
            }
            return null;
        }
    }
}
