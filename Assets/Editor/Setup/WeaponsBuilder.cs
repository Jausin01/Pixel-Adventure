using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Creates every weapon and the holder that manages them.
    /// </summary>
    public static class WeaponsBuilder
    {
        /// <summary>Builds the weapons and returns the holder.</summary>
        public static WeaponHolder Build(Material material, GameObject player, out SwordVisual swordVisual)
        {
            HitStop hitStop = new GameObject("HitStop").AddComponent<HitStop>();

            GameObject sword = SwordBuilder.Build(material, player, hitStop);
            GameObject bow = BowBuilder.Build(material, player);
            GameObject axe = AxeBuilder.Build(material, player, hitStop);
            GameObject spear = SpearBuilder.Build(material, player, hitStop);
            GameObject daggers = DaggerBuilder.Build(material, player, hitStop);
            GameObject hammer = HammerBuilder.Build(material, player, hitStop);
            GameObject staff = StaffBuilder.Build(material, player, hitStop);
            swordVisual = sword.GetComponent<SwordVisual>();

            GameObject holderObject = new GameObject("Weapons");
            WeaponHolder holder = holderObject.AddComponent<WeaponHolder>();
            holder.weapons = new[] { sword, bow, axe, spear, daggers, hammer, staff };
            return holder;
        }
    }
}
