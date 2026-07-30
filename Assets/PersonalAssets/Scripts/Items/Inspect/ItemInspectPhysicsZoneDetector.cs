namespace ExoLab.Assembly
{
    using ExoLab.Constants;
    using UnityEngine;

    /// <summary>
    /// Detects whether the cursor is in the assembly zone using 3D physics raycasting.
    /// Used when the assembly zone is a 3D collider in the scene.
    /// </summary>
    public sealed class ItemInspectPhysicsZoneDetector : IItemInspectZoneDetector
    {
        private readonly Camera inspectCamera;

        public ItemInspectPhysicsZoneDetector(Camera inspectCamera)
        {
            this.inspectCamera = inspectCamera;
        }

        public bool IsCursorInZone()
        {
            var ray = this.inspectCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hit) == false)
                return false;

            return hit.collider.gameObject.CompareTag(Constants.Tags.AssemblyZone);
        }
    }
}