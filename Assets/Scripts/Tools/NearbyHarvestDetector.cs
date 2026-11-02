using System;
using UnityEngine;

namespace MobileRpg
{
    /// <summary>
    /// Watches for harvestable resources near this object and reports which tool they need.
    /// </summary>
    public class NearbyHarvestDetector : MonoBehaviour
    {
        [Tooltip("How far around the object resources are detected.")]
        public float range = 1.5f;

        [Tooltip("Seconds between scans.")]
        public float scanInterval = 0.1f;

        /// <summary>Tool needed by the nearest resource, or <see cref="ToolType.None"/> when there is none.</summary>
        public ToolType CurrentTool { get; private set; }

        /// <summary>Raised when <see cref="CurrentTool"/> changes.</summary>
        public event Action<ToolType> ToolChanged;

        private float timer;

        private void Update()
        {
            timer -= Time.deltaTime;
            if (timer > 0f) return;
            timer = scanInterval;

            IHarvestable nearest = HarvestTargetFinder.FindNearest(transform.position, range, ToolType.None);
            ToolType tool = nearest != null ? nearest.RequiredTool : ToolType.None;
            if (tool == CurrentTool) return;

            CurrentTool = tool;
            if (ToolChanged != null) ToolChanged(tool);
        }
    }
}
