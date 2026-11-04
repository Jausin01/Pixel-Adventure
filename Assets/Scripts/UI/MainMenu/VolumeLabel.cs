using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Shows a volume setting as a percentage, for example "Music 70%".
    /// </summary>
    public class VolumeLabel : MonoBehaviour
    {
        public Text text;
        public string title = "Music";
        public bool music = true;

        private void OnEnable()
        {
            VolumeSettings.Changed += Refresh;
            Refresh();
        }

        private void OnDisable() { VolumeSettings.Changed -= Refresh; }

        private void Refresh()
        {
            float value = music ? VolumeSettings.Music : VolumeSettings.Sfx;
            text.text = title + " " + Mathf.RoundToInt(value * 100f) + "%";
        }
    }
}
