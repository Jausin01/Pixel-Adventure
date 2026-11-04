using System.Globalization;
using System.Text;

namespace MobileRpg
{
    /// <summary>
    /// Builds readable text from item data.
    /// </summary>
    public static class ItemDescriber
    {
        /// <summary>Returns a short line listing what the item does when used, or an empty string.</summary>
        public static string Effects(ItemDefinition item)
        {
            StringBuilder text = new StringBuilder();
            if (item.healthRestored > 0f) text.Append("Restores ").Append((int)item.healthRestored).Append(" HP");

            if (item.energyRestored > 0f)
            {
                if (text.Length > 0) text.Append("\n");
                text.Append("Restores ").Append((int)item.energyRestored).Append(" energy");
            }

            AppendBonus(text, "ATK", item.attackBonus);
            AppendBonus(text, "DEF", item.defenseBonus);
            AppendBonus(text, "HP", item.maxHealthBonus);
            return text.ToString();
        }

        /// <summary>
        /// Like <see cref="Effects(ItemDefinition)"/> for one inventory slot: pieces of gear show their own final bonuses,
        /// after level, reforges and nature.
        /// </summary>
        public static string Effects(InventorySlot slot)
        {
            if (slot.Gear == null) return Effects(slot.Item);

            StringBuilder text = new StringBuilder();
            AppendBonus(text, "ATK", slot.Gear.Attack);
            AppendBonus(text, "DEF", slot.Gear.Defense);
            AppendBonus(text, "HP", slot.Gear.MaxHealth);
            return text.ToString();
        }

        private static void AppendBonus(StringBuilder text, string label, float value)
        {
            if (value == 0f) return;
            if (text.Length > 0) text.Append("\n");
            text.Append(value > 0f ? "+" : string.Empty).Append(value.ToString("0.#", CultureInfo.InvariantCulture)).Append(" ").Append(label);
        }

        /// <summary>Returns the category as text for display.</summary>
        public static string CategoryName(ItemCategory category)
        {
            return category.ToString();
        }
    }
}
