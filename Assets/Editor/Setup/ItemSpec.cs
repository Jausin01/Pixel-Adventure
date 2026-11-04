namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Describes an item for <see cref="ItemAssetFactory"/>.
    /// </summary>
    public class ItemSpec
    {
        public string id;
        public string displayName;
        public string iconPath;          // relative to the art folder, for example "Items/wood"
        public ItemCategory category;
        public string tierId;            // matches a tier created by ItemCatalogBuilder
        public string description;
        public float healthRestored;
        public float energyRestored;
        public EquipmentKind equipKind;
        public float attack;
        public float defense;
        public float maxHealth;
        public int sellValue;            // what the merchant pays for one, in the currency
        public int maxStack;             // 0 keeps the default of the item definition
        public bool dropAsSingleStack;
        public int swordSkinIndex;       // index into the sword skins (silver, blue, red, black, green)
    }
}
