namespace ExoLab.Assembly
{
    using System.Collections.Generic;
    using System.Linq;
    using ExoLab.Constants;
    using UnityEngine;
    using UnityEngine.EventSystems;
    using UnityEngine.UI;

    /// <summary>
    /// Detects whether the cursor is in the assembly zone using UI raycasting.
    /// Used when the assembly zone is a UI element.
    /// </summary>
    public sealed class ItemInspectGraphicZoneDetector : IItemInspectZoneDetector
    {
        private readonly GraphicRaycaster graphicRaycaster;
        private readonly PointerEventData pointerData;
        private readonly List<RaycastResult> resultsCache = new();

        public ItemInspectGraphicZoneDetector(GraphicRaycaster graphicRaycaster, EventSystem eventSystem)
        {
            this.graphicRaycaster = graphicRaycaster;
            this.pointerData = new PointerEventData(eventSystem);
        }

        public bool IsCursorInZone()
        {
            resultsCache.Clear();
            pointerData.position = Input.mousePosition;
            graphicRaycaster.Raycast(pointerData, resultsCache);

            // If there's nothing hit, or more than one hit (meaning something else is under cursor),
            // we consider it outside the zone.
            if (resultsCache.Count == 0 || resultsCache.Count > 1)
                return false;

            return resultsCache.Any(target => target.gameObject.CompareTag(Constants.Tags.AssemblyZone));
        }
    }
}