using UnityEngine;

namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Gives the player the ability to wear gear: the equipment itself, the final stats, and the links that
    /// apply those stats to health and weapon damage, and pick the weapon in hand and its look.
    /// Needs the inventory, health and the weapons to exist already.
    /// </summary>
    public static class EquipmentBuilder
    {
        /// <summary>Adds the equipment components to the player.</summary>
        public static Equipment Attach(GameObject player, WeaponHolder weapons)
        {
            Equipment equipment = player.AddComponent<Equipment>();

            Health health = player.GetComponent<Health>();
            CharacterStats stats = player.AddComponent<CharacterStats>();
            stats.baseMaxHealth = health.maxHealth;


            HealthStatsLink healthLink = player.AddComponent<HealthStatsLink>();
            healthLink.stats = stats;
            healthLink.health = health;

            WeaponDamageLink damageLink = player.AddComponent<WeaponDamageLink>();
            damageLink.stats = stats;
            damageLink.holder = weapons;

            EquippedWeaponLink weaponLink = player.AddComponent<EquippedWeaponLink>();
            weaponLink.equipment = equipment;
            weaponLink.holder = weapons;

            SwordSkinLink skinLink = player.AddComponent<SwordSkinLink>();
            skinLink.equipment = equipment;
            skinLink.visual = weapons.weapons[0].GetComponent<SwordVisual>();
            return equipment;
        }
    }
}
