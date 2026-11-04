using System;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// The character's final numbers: base values plus the bonuses of everything worn.
    /// Other components read these values and apply them; this class changes nothing itself.
    /// </summary>
    [RequireComponent(typeof(Equipment))]
    public class CharacterStats : MonoBehaviour
    {
        [Header("Base values (without gear)")]
        public float baseMaxHealth = 100f;
        public float baseDefense = 0f;
        public float baseSwordAttack = 10f;
        public float baseAxeAttack = 15f;
        public float baseSpearAttack = 11f;
        public float baseBowAttack = 8f;
        public float baseDaggerAttack = 6f;
        public float baseHammerAttack = 20f;
        public float baseStaffAttack = 9f;
        public float baseSecondaryAttack = 6f;

        private Equipment equipment;

        /// <summary>Raised when any final value may have changed.</summary>
        public event Action Changed;

        /// <summary>Maximum hit points.</summary>
        public float MaxHealth
        {
            get
            {
                float total = baseMaxHealth;
                for (int i = 0; i < EquipmentSlots.All.Length; i++) total += Bonus(EquipmentSlots.All[i], BonusKind.MaxHealth);
                return total;
            }
        }

        /// <summary>Damage reduction.</summary>
        public float Defense
        {
            get
            {
                float total = baseDefense;
                for (int i = 0; i < EquipmentSlots.All.Length; i++) total += Bonus(EquipmentSlots.All[i], BonusKind.Defense);
                return total;
            }
        }

        /// <summary>Damage of the sword: base, plus attack of worn gear, plus the sword's own attack.</summary>
        public float SwordAttack { get { return AttackFor(WeaponClass.Sword); } }

        /// <summary>Damage of the secondary weapon (shuriken, boomerang): base, plus worn gear, plus its own attack.</summary>
        public float SecondaryAttack { get { return baseSecondaryAttack + GearAttack() + Bonus(EquipmentSlot.Secondary, BonusKind.Attack); } }

        /// <summary>Damage of a main weapon class: its base, plus attack of worn gear, plus the attack of the worn weapon.</summary>
        public float AttackFor(WeaponClass weaponClass)
        {
            return BaseAttack(weaponClass) + GearAttack() + Bonus(EquipmentSlot.Weapon, BonusKind.Attack);
        }

        private enum BonusKind { Attack, Defense, MaxHealth }

        private void Awake()
        {
            equipment = GetComponent<Equipment>();
        }

        private void OnEnable()
        {
            if (equipment == null) equipment = GetComponent<Equipment>();
            equipment.Changed += RaiseChanged;
        }

        private void OnDisable()
        {
            equipment.Changed -= RaiseChanged;
        }

        private float BaseAttack(WeaponClass weaponClass)
        {
            switch (weaponClass)
            {
                case WeaponClass.Axe: return baseAxeAttack;
                case WeaponClass.Spear: return baseSpearAttack;
                case WeaponClass.Bow: return baseBowAttack;
                case WeaponClass.Dagger: return baseDaggerAttack;
                case WeaponClass.Hammer: return baseHammerAttack;
                case WeaponClass.Staff: return baseStaffAttack;
                default: return baseSwordAttack;
            }
        }

        // Attack from everything except the weapons themselves (armor, accessories).
        private float GearAttack()
        {
            float total = 0f;
            for (int i = 0; i < EquipmentSlots.All.Length; i++)
            {
                if (!EquipmentSlots.IsWeaponSlot(EquipmentSlots.All[i])) total += Bonus(EquipmentSlots.All[i], BonusKind.Attack);
            }
            return total;
        }

        private float Bonus(EquipmentSlot slot, BonusKind kind)
        {
            if (equipment == null) equipment = GetComponent<Equipment>();

            InventorySlot piece = equipment.GetPiece(slot);
            if (piece == null || piece.Gear == null) return 0f;

            switch (kind)
            {
                case BonusKind.Attack: return piece.Gear.Attack;
                case BonusKind.Defense: return piece.Gear.Defense;
                default: return piece.Gear.MaxHealth;
            }
        }

        private void RaiseChanged()
        {
            if (Changed != null) Changed();
        }
    }
}
