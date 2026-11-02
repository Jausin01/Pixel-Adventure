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

            return text.ToString();
        }

        /// <summary>Returns the category as text for display.</summary>
        public static string CategoryName(ItemCategory category)
        {
            return category.ToString();
        }
    }
}
