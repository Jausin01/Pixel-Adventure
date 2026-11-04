using System.Collections;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Opens the screen on the same object a moment after the game starts (used by the main menu).
    /// </summary>
    [RequireComponent(typeof(InventoryPanel))]
    public class OpenOnStart : MonoBehaviour
    {
        private IEnumerator Start()
        {
            yield return null; // let the panel hide its window first
            GetComponent<InventoryPanel>().Open();
        }
    }
}
