using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Makes the weapon in hand match the item worn in the Weapon slot: selects the weapon of its class and
    /// gives it the item's look. With nothing worn, the sword is used.
    /// </summary>
    public class EquippedWeaponLink : MonoBehaviour
    {
        public Equipment equipment;
        public WeaponHolder holder;

        private void OnEnable() { equipment.Changed += Apply; }

        private void OnDisable() { equipment.Changed -= Apply; }

        private void Start() { Apply(); }

        private void Apply()
        {
            ItemDefinition item = equipment.Get(EquipmentSlot.Weapon);

            WeaponClass weaponClass = WeaponClass.Sword;
            if (item != null) EquipmentSlots.TryGetWeaponClass(item.equipKind, out weaponClass);
            holder.Select(weaponClass);

            GameObject weapon = holder.Find(weaponClass);
            WeaponSkinSet skins = weapon != null ? weapon.GetComponent<WeaponSkinSet>() : null;
            if (skins != null) skins.SetSkin(item != null ? item.swordSkinIndex : 0);
        }
    }
}
