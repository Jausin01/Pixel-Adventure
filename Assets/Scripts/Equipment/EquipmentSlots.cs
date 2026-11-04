namespace MobileRpg
{
    /// <summary>
    /// Facts about the equipment slots: their order, names and which gear fits in them.
    /// </summary>
    public static class EquipmentSlots
    {
        /// <summary>Every slot, in display order.</summary>
        public static readonly EquipmentSlot[] All =
        {
            EquipmentSlot.Weapon, EquipmentSlot.Secondary, EquipmentSlot.Head, EquipmentSlot.Body,
            EquipmentSlot.Legs, EquipmentSlot.Accessory1, EquipmentSlot.Accessory2
        };

        /// <summary>True when the item can be worn in the slot.</summary>
        public static bool Accepts(EquipmentSlot slot, ItemDefinition item)
        {
            if (item == null || item.equipKind == EquipmentKind.None) return false;

            EquipmentKind kind = item.equipKind;
            switch (slot)
            {
                case EquipmentSlot.Weapon:
                    WeaponClass ignored;
                    return TryGetWeaponClass(kind, out ignored);
                case EquipmentSlot.Secondary: return kind == EquipmentKind.Shuriken || kind == EquipmentKind.Boomerang || kind == EquipmentKind.Bow;
                case EquipmentSlot.Head: return kind == EquipmentKind.Head;
                case EquipmentSlot.Body: return kind == EquipmentKind.Body;
                case EquipmentSlot.Legs: return kind == EquipmentKind.Legs;
                default: return kind == EquipmentKind.Accessory;
            }
        }

        /// <summary>The weapon class of a main weapon kind. False for gear that is not a main weapon.</summary>
        public static bool TryGetWeaponClass(EquipmentKind kind, out WeaponClass weaponClass)
        {
            switch (kind)
            {
                case EquipmentKind.Sword: weaponClass = WeaponClass.Sword; return true;
                case EquipmentKind.Axe: weaponClass = WeaponClass.Axe; return true;
                case EquipmentKind.Spear: weaponClass = WeaponClass.Spear; return true;
                case EquipmentKind.Bow: weaponClass = WeaponClass.Bow; return true;
                case EquipmentKind.Dagger: weaponClass = WeaponClass.Dagger; return true;
                case EquipmentKind.Hammer: weaponClass = WeaponClass.Hammer; return true;
                case EquipmentKind.Staff: weaponClass = WeaponClass.Staff; return true;
                default: weaponClass = WeaponClass.Sword; return false;
            }
        }

        /// <summary>True for the slots that hold weapons (their attack only counts for that weapon).</summary>
        public static bool IsWeaponSlot(EquipmentSlot slot)
        {
            return slot == EquipmentSlot.Weapon || slot == EquipmentSlot.Secondary;
        }

        /// <summary>Name shown to the player.</summary>
        public static string Label(EquipmentSlot slot)
        {
            switch (slot)
            {
                case EquipmentSlot.Accessory1: return "Accessory 1";
                case EquipmentSlot.Accessory2: return "Accessory 2";
                default: return slot.ToString();
            }
        }
    }
}
