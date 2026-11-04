using UnityEngine;
using UnityEngine.UI;

namespace MobileRpg
{
    /// <summary>
    /// Shows the skills of one weapon class at a time on the skill teacher's screen. Each tab owns a group of rows.
    /// </summary>
    public class SkillClassTabs : MonoBehaviour
    {
        public GameObject[] groups;
        public Image[] tabBackgrounds;
        public Color normalColor = new Color(0.25f, 0.18f, 0.14f, 1f);
        public Color activeColor = new Color(0.55f, 0.38f, 0.2f, 1f);

        private int selected;

        private void OnEnable()
        {
            Select(selected);
        }

        /// <summary>Shows the group at an index and highlights its tab.</summary>
        public void Select(int index)
        {
            selected = index;
            for (int i = 0; i < groups.Length; i++)
            {
                groups[i].SetActive(i == index);
                tabBackgrounds[i].color = i == index ? activeColor : normalColor;
            }
        }
    }
}
