namespace MobileRpg.EditorTools
{
    /// <summary>
    /// Values shared by the editor setup scripts and the art importer.
    /// </summary>
    public static class ProjectConstants
    {
        /// <summary>Pixels per world unit for pixel art. One 16x16 cell equals one unit.</summary>
        public const int PixelsPerUnit = 16;

        /// <summary>Pixel Perfect Camera reference width (16 x 9 aspect, 16 units wide).</summary>
        public const int ReferenceWidth = 256;

        /// <summary>Pixel Perfect Camera reference height.</summary>
        public const int ReferenceHeight = 144;

        /// <summary>Folder that contains all art.</summary>
        public const string ArtFolder = "Assets/Art";

        /// <summary>Id of the item used as money. Rename the item's display name in ItemCatalogBuilder.</summary>
        public const string CurrencyId = "rift_shard";

        /// <summary>Path of the town scene, where the player lives between runs.</summary>
        public const string ScenePath = "Assets/Scenes/Town.unity";

        /// <summary>Folder that holds one scene per arena.</summary>
        public const string ArenaSceneFolder = "Assets/Scenes/Arenas";

        /// <summary>Prefix of arena scene names; the arena id follows.</summary>
        public const string ArenaScenePrefix = "Arena_";
    }
}
