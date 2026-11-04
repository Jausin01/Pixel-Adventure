using System;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Remembers which equipment slot is selected on the equipment screen and tells the views when it changes.
    /// Must stay on an object that is always active.
    /// </summary>
    public class EquipmentBrowser : MonoBehaviour
    {
        /// <summary>Raised when the selection changes.</summary>
        public event Action Changed;

        /// <summary>The selected slot.</summary>
        public EquipmentSlot Selected { get; private set; }

        /// <summary>Selects a slot.</summary>
        public void Select(EquipmentSlot slot)
        {
            Selected = slot;
            if (Changed != null) Changed();
        }
    }
}
