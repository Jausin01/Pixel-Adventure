using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Shows the sword that is equipped: picks the sword skin chosen by the item, or the first skin when none is worn.
    /// </summary>
    public class SwordSkinLink : MonoBehaviour
    {
        public Equipment equipment;
        public SwordVisual visual;

        private void OnEnable() { equipment.Changed += Apply; }

        private void OnDisable() { equipment.Changed -= Apply; }

        private void Start() { Apply(); }

        private void Apply()
        {
            ItemDefinition sword = equipment.Get(EquipmentSlot.Weapon);
            visual.SetSkin(sword != null ? sword.swordSkinIndex : 0);
        }
    }
}
