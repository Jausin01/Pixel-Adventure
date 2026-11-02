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
    }
}
