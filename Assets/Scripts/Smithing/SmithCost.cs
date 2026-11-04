using System.Text;

namespace MobileRpg
{
    /// <summary>
    /// What the blacksmith asks for one job: materials and money.
    /// </summary>
    public class SmithCost
    {
        public ItemAmount[] materials;
        public int shards;

        /// <summary>The cost as one line, each part green when the player has enough and red when not.</summary>
        public string Describe(Inventory inventory, ItemDefinition currency)
        {
            StringBuilder text = new StringBuilder();
            for (int i = 0; i < materials.Length; i++)
            {
                Append(text, materials[i].item.displayName, inventory.Count(materials[i].item), materials[i].amount);
            }
            Append(text, currency.displayName, inventory.Count(currency), shards);
            return text.ToString();
        }

        private static void Append(StringBuilder text, string name, int have, int need)
        {
            if (need <= 0) return;
            if (text.Length > 0) text.Append("   ");

            string color = have >= need ? "#9be58a" : "#ff8a7a";
            text.Append("<color=").Append(color).Append(">").Append(name).Append(" ").Append(have).Append("/").Append(need).Append("</color>");
        }
    }
}
