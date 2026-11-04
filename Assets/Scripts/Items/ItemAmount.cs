using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// An item and a quantity. Used wherever data lists "N of this item" (quest costs and rewards).
    /// </summary>
    [System.Serializable]
    public class ItemAmount
    {
        public ItemDefinition item;
        public int amount = 1;
    }
}
